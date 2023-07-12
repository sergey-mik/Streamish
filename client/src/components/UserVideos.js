import React, { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import Video from './Video'
import { getUserVideos } from '../modules/videoManager'

const UserVideos = () => {
  const [videos, setVideos] = useState([])
  const { id } = useParams()

  useEffect(() => {
    getUserVideos(id).then(setVideos)
  }, [id])

  return (
    <div className="container">
      <div className="row justify-content-center">
        {videos.map((video) => {
          if (!video) {
            return null
          }
          return (
            <div className="col-sm-12 col-lg-6">
              <Video video={video} key={video.id} />
            </div>
          )
        })}
      </div>
    </div>
  )
}

export default UserVideos
