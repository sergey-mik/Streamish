import React from 'react'
import './App.css'
import VideoList from './components/VideoList'
import SearchVideos from './components/Search'

function App() {
  return (
    <div className="App">
      <SearchVideos />
      <VideoList />
    </div>
  )
}

export default App
