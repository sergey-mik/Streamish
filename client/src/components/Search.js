import React, { useState } from 'react'
import { Button, Form, FormGroup, Input, InputGroup, InputGroupText } from 'reactstrap'
import { videoSearch } from '../modules/videoManager'

const SearchVideos = () => {
  const [query, setQuery] = useState('')
  const [videos, setVideos] = useState([])

  const handleSearch = async (event) => {
    event.preventDefault()

    // Don't fetch results if search input is empty
    if (query.trim() === '') {
      setVideos([])
      return
    }

    // Fetch search results from API using videoSearch function
    const videos = await videoSearch(query, false)

    // Update state with search results
    setVideos(videos)
  }

  return (
    <div>
      <div className="form-container">
        <Form onSubmit={handleSearch}>
          <FormGroup>
            <InputGroup>
              <Input
                type="text"
                placeholder="Search videos"
                value={query}
                onChange={(event) => setQuery(event.target.value)}
              />
              <InputGroupText>
                <Button type="submit">Search</Button>
              </InputGroupText>
            </InputGroup>
          </FormGroup>
        </Form>
      </div>

      {/* Render search results */}
      <div>
        {videos.map((video) => (
          <div key={video.id}>
            <h4>{video.title}</h4>
          </div>
        ))}
      </div>

      <style jsx>{`
        .form-container {
          max-width: 500px;
          margin: 0 auto;
        }
      `}</style>
    </div>
  )
}

export default SearchVideos
