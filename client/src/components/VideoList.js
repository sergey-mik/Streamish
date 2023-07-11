import React, { useEffect, useState } from 'react'
import Video from './Video'
import FormComponent from './Form'
import { getAllVideos } from '../modules/videoManager'

const VideoList = () => {
  const [videos, setVideos] = useState([])

  const getVideos = () => {
    getAllVideos().then((videos) => setVideos(videos))
  }

  useEffect(() => {
    getVideos()
  }, [])

  return (
    <div className="container">
      {/* Pass getVideos function as prop to FormComponent */}
      <FormComponent onVideoCreated={getVideos} />

      <div className="row justify-content-center">
        {videos.map((video) => (
          <Video video={video} key={video.id} />
        ))}
      </div>
    </div>
  )
}

export default VideoList
