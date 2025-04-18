import React, { useEffect, useState } from 'react'
import axios from 'axios'

function Genres({ selectedGenres, onToggleGenre }) {

  const [genres, setGenres] = useState([]);

  useEffect(() => {

    async function fetchGenres() {
      try {
        const res = await axios.get("http://localhost:8080/api/genres");
        // console.log(res.data);
        setGenres(res.data);
      }
      catch (err) {
        console.error("Failed to fetch genres:", err)
      }
    }

    fetchGenres()
  }, [])


  return (
    <>
      <div className="mt-6 flex flex-wrap gap-2 justify-center">

        {genres.map((genre => {
          const isSelected = selectedGenres.includes(genre.name);
          return (
            <button
              key={genre.id}
              onClick={() => onToggleGenre(genre.name)}
              className={`py-1 px-2 rounded-lg font-bold text-lg text-white transition 
                ${isSelected ? 'bg-blue-500 hover:bg-blue-600' : 'bg-gray-400 hover:bg-blue-400'}`}
            > {genre.name}</button>
          )
        }))}

      </div>

    </>
  )
}

export default Genres