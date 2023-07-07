using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Streamish.Models;
using System.Collections.Generic;
using System.Linq;

namespace Streamish.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration configuration) : base(configuration) { }

        public IEnumerable<UserProfile> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name, Email, DateCreated FROM UserProfile";
                    using (var reader = cmd.ExecuteReader())
                    {
                        var userProfiles = new List<UserProfile>();
                        while (reader.Read())
                        {
                            userProfiles.Add(new UserProfile()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated"))
                            });
                        }
                        return userProfiles;
                    }
                }
            }
        }

        public UserProfile GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name, Email, DateCreated FROM UserProfile WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UserProfile()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated"))
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public void Add(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO UserProfile (Name, Email, DateCreated) OUTPUT INSERTED.Id VALUES (@Name, @Email, @DateCreated)";
                    cmd.Parameters.AddWithValue("@Name", userProfile.Name);
                    cmd.Parameters.AddWithValue("@Email", userProfile.Email);
                    cmd.Parameters.AddWithValue("@DateCreated", userProfile.DateCreated);
                    userProfile.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "UPDATE UserProfile SET Name = @Name, Email = @Email, DateCreated = @DateCreated WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", userProfile.Id);
                    cmd.Parameters.AddWithValue("@Name", userProfile.Name);
                    cmd.Parameters.AddWithValue("@Email", userProfile.Email);
                    cmd.Parameters.AddWithValue("@DateCreated", userProfile.DateCreated);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM UserProfile WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public UserProfile GetUserProfileWithVideos(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT up.Id AS UserProfileId, up.Name, up.Email, up.DateCreated AS UserProfileDateCreated,
                       v.Id AS VideoId, v.Title, v.Description, v.Url, v.DateCreated AS VideoDateCreated,
                       c.Id AS CommentId, c.Message, c.UserProfileId AS CommentUserProfileId
                  FROM UserProfile up
                       LEFT JOIN Video v ON v.UserProfileId = up.Id
                       LEFT JOIN Comment c ON c.VideoId = v.Id
                 WHERE up.Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        UserProfile userProfile = null;
                        while (reader.Read())
                        {
                            if (userProfile == null)
                            {
                                userProfile = new UserProfile()
                                {
                                    Id = id,
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    DateCreated = reader.GetDateTime(reader.GetOrdinal("UserProfileDateCreated")),
                                    Videos = new List<Video>()
                                };
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("VideoId")))
                            {
                                var videoId = reader.GetInt32(reader.GetOrdinal("VideoId"));
                                var video = userProfile.Videos.FirstOrDefault(v => v.Id == videoId);
                                if (video == null)
                                {
                                    video = new Video()
                                    {
                                        Id = videoId,
                                        Title = reader.GetString(reader.GetOrdinal("Title")),
                                        Description = reader.GetString(reader.GetOrdinal("Description")),
                                        Url = reader.GetString(reader.GetOrdinal("Url")),
                                        DateCreated = reader.GetDateTime(reader.GetOrdinal("VideoDateCreated")),
                                        UserProfileId = id,
                                        Comments = new List<Comment>()
                                    };
                                    userProfile.Videos.Add(video);
                                }

                                if (!reader.IsDBNull(reader.GetOrdinal("CommentId")))
                                {
                                    video.Comments.Add(new Comment()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("CommentId")),
                                        Message = reader.GetString(reader.GetOrdinal("Message")),
                                        VideoId = videoId,
                                        UserProfileId = reader.GetInt32(reader.GetOrdinal("CommentUserProfileId"))
                                    });
                                }
                            }
                        }

                        return userProfile;
                    }
                }
            }
        }
    }
}
