import React from 'react'
import { Card, CardBody } from 'reactstrap'
import { Link } from 'react-router-dom'

const Video = ({ video }) => {
  return (
    <Card>
      <p className="text-left px-2">
        Posted by:{' '}
        {video.userProfile ? (
          <Link to={`/videos/users/${video.userProfile.id}`}>
            {video.userProfile.name}
          </Link>
        ) : (
          'User not available'
        )}
      </p>
      <CardBody>
        <iframe
          className="video"
          src={video.url}
          title="YouTube video player"
          frameBorder="0"
          allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
          allowFullScreen
        />

        <Link to={`/videos/${video.id}`} className="text-center d-block">
          <strong>{video.title}</strong>
        </Link>

        <p>{video.description}</p>

        <h4>Comments</h4>
        {video.comments &&
          video.comments.map((comment) => (
            <p key={comment.id}>{comment.message}</p>
          ))}
      </CardBody>
    </Card>
  )
}

export default Video
