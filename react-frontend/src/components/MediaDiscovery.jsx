import React, { useEffect, useState } from 'react'
import axios from 'axios'
import getImageUrl from '../Utils/GetImageUrl'
import { Link } from 'react-router-dom'

function MediaDiscovery() {
  const [mediaList, setMediaList] = useState([])

  useEffect(() => {
    async function fetchMedia()
    {
      try
      {
        const res = await axios.get("http://localhost:8080/api/media/newReleases");
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
          <Link to={`/${media.type === 'TvShow' ? 'shows' : 'movies'}/${media.id}`} state={{ type: media.type }}>
            <div
              key={index}
              className="w-[15vw] h-[25vh] m-5 rounded-lg bg-cover bg-center flex flex-col justify-end"
              style={{ backgroundImage: `url(${getImageUrl(media.posterImage)})` }}
              title= { media.title}
            >
              <div className="text-xl text-white bg-gray-900 bg-opacity-60 p2 text-center w-full h-[4vh] rounded-lg">
                {truncateTitle(media.title)}
              </div>
            </div>
          </Link>
        ))}
      </div>
    </div>
  )
}

export default MediaDiscovery
