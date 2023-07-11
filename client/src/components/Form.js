import React, { useState } from 'react'
import { Alert, Button, Form, FormGroup, Label, Input } from 'reactstrap'
import { addVideo } from '../modules/videoManager'

const FormComponent = ({ onVideoCreated }) => {
  const [formData, setFormData] = useState({
    title: '',
    description: '',
    url: '',
  })
  const [status, setStatus] = useState(null)

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value })
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    // Use the addVideo function from the VideoManager module to send a POST request to your backend API
    const response = await addVideo(formData)
    if (response.ok) {
      // Handle successful video creation here
      setStatus('success')
      // Call onVideoCreated callback function to refresh list of videos in parent component
      onVideoCreated()
    } else {
      // Handle errors here
      setStatus('error')
    }
  }

  return (
    <>
      {status === 'success' && (
        <Alert color="success">Video created successfully!</Alert>
      )}
      {status === 'error' && (
        <Alert color="danger">
          An error occurred while creating the video.
        </Alert>
      )}
      <div className="form-container">
        <Form onSubmit={handleSubmit}>
          <FormGroup>
            <Label for="title">Title:</Label>
            <Input
              type="text"
              name="title"
              id="title"
              value={formData.title}
              onChange={handleChange}
            />
          </FormGroup>
          <FormGroup>
            <Label for="description">Description:</Label>
            <Input
              type="text"
              name="description"
              id="description"
              value={formData.description}
              onChange={handleChange}
            />
          </FormGroup>
          <FormGroup>
            <Label for="url">URL:</Label>
            <Input
              type="text"
              name="url"
              id="url"
              value={formData.url}
              onChange={handleChange}
            />
          </FormGroup>
          <Button type="submit">Submit</Button>
        </Form>
        <style jsx>{`
          .form-container {
            max-width: 500px;
            margin: 0 auto;
          }
        `}</style>
      </div>
    </>
  )
}

export default FormComponent
