import React, { useEffect, useState } from 'react'
import axios from 'axios'
import Genres from './Genres'
import MediaDisplayTable from './MediaDisplayTable';
import { useAuth } from './contexts/AuthContext';
import { useWatchlist } from './contexts/WatchlistContext';

function Shows() {
  const [shows, setShows] = useState([]);
  useEffect(() => {
    async function fetchMedia() {
      try {
        const res = await axios.get("http://localhost:8080/api/media/shows");
        // console.log(res.data);
        setShows(res.data)
      }
      catch (err) {
        console.error("Failed to fetch shows:", err)
      }
    }

    fetchMedia()
  }, [])

  const [selectedGenres, setSelectedGenres] = useState([]);
  function handleGenreToggle(genreName) {
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

  const { watchlistDict: watchlistDict, ChangeElement: handleRefresh} = useWatchlist();

  return (
    <>
      <Genres selectedGenres={selectedGenres} onToggleGenre={handleGenreToggle}></Genres>
      <MediaDisplayTable selectedGenres={selectedGenres} media={shows} watchlistDict={watchlistDict} handleWatclistChange={handleRefresh} ></MediaDisplayTable>
    </>
  )
}

export default Shows