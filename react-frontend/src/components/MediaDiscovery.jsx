import React, { useEffect, useState } from 'react'
import Banner from '../images/banner.jpg'
import axios from 'axios'

function MediaDiscovery() {
  const [mediaList, setMediaList] = useState([])

  useEffect(() => {
    async function fetchMedia()
    {
      try
      {
        const res = await axios.get("http://localhost:8080/api/media");
        // console.log(res.data);
        setMediaList(res.data);
      }
      catch (err)
      {
        console.error("Failed to fetch media:", err)
      }
    }

    fetchMedia()
  }, [])

  function getImageUrl(posterImage)
  {
    // Ensure that a random NULL value won't cause issues
    if (posterImage && posterImage.length > 0)
    {
      const base64String = btoa(
        posterImage.reduce((data, byte) => data + String.fromCharCode(byte), '')
      )
      return `data:image/jpeg;base64,${base64String}`
    } 
    else
      return Banner
  }

  function truncateTitle(title)
  {
    const maxLength = 20; // TBD: reducing
    return title.length > maxLength 
      ? title.slice(0, maxLength) + "..." 
      : title
  }

  return (
    <div>
      <div className="mb-8 font-bold text-4xl text-center">
        New releases
      </div>

      <div className="flex justify-center flex-wrap">
        {mediaList.map((media, index) => (
          <div
            key={index}
            className="w-[15vw] h-[25vh] m-5 rounded-lg bg-cover bg-center flex flex-col justify-end"
            style={{ backgroundImage: `url(${getImageUrl(media.PosterImage)})` }}
            title= { media.title}
          >
            <div className="text-xl text-white bg-gray-900 bg-opacity-60 p2 text-center w-full h-[4vh] rounded-lg">
              {truncateTitle(media.title)}
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}

export default MediaDiscovery
