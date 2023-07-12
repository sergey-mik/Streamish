const baseUrl = '/api/video'
const userProfileBaseUrl = '/api/UserProfile'

export const getAllVideos = () => {
  return fetch(baseUrl + '/getwithcomments').then((res) => res.json())
}

export const addVideo = (video) => {
  return fetch(baseUrl, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(video),
  })
}

export const videoSearch = (searchString, sortDesc) => {
  return fetch(baseUrl + `/search?q=${searchString}`).then((res) => res.json())
}

export const getVideo = (id) => {
  return fetch(`${baseUrl}/${id}`).then((res) => res.json())
}

// export const getVideo = (id) => {
//   return fetch(`${baseUrl}/getwithcomments?id${id}`).then((res) => res.json())
// }

export const getUserVideos = (id) => {
  return fetch(`${userProfileBaseUrl}/${id}/videos`)
    .then((res) => res.json())
    .then((data) => data.videos)
}

