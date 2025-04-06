import React, { useEffect, useState } from 'react'
import axios from 'axios'
import Genres from './Genres'
import MediaDisplayTable from './MediaDisplayTable';

function Movies() {

  const [movies, setMovies] = useState([]);

  useEffect(() => {
    async function fetchMedia()
    {
      try
      {
        const res = await axios.get("http://localhost:8080/api/media/movies");
        // console.log(res.data);
        setMovies(res.data)
      }
      catch (err)
      {
        console.error("Failed to fetch movies:", err)
      }
    }

    fetchMedia()
  }, [])

  const [selectedGenres, setSelectedGenres] = useState([]);

  function handleGenreToggle(genreName)
  {
    setSelectedGenres((prev) => {
      // console.log(prev)
      if (prev.includes(genreName)) {
        // Remove genre if it's already selected
        return prev.filter((g) => g !== genreName);
      } else {
        // Add genre if it's not selected
        return [...prev, genreName];
      }
    });
  };


  return (


    <>
      <Genres selectedGenres={selectedGenres} onToggleGenre={handleGenreToggle}></Genres>
      <MediaDisplayTable selectedGenres={selectedGenres} media={movies} mediaType={"movies"}></MediaDisplayTable>
    </>
  )
}

export default Movies