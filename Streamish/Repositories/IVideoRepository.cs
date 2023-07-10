using Streamish.Models;
using System;
using System.Collections.Generic;

namespace Streamish.Repositories
{
    public interface IVideoRepository
    {
        List<Video> GetAll();
        List<Video>GetAllWithComments();
        Video GetById(int id);
        void Add(Video video);
        void Update(Video video);
        void Delete(int id);
        List<Video> Search(string criterion, bool sortDescending);
        List<Video> GetHottestVideos(DateTime since);
    }
}
