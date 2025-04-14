import React, { useEffect, useState } from 'react'
import axios from 'axios'
import Genres from './Genres'
import MediaDisplayTable from './MediaDisplayTable';

function Movies() {

  const [movies, setMovies] = useState([]);

  useEffect(() => {
    async function fetchMedia() {
      try {
        const res = await axios.get("http://localhost:8080/api/media/movies");
        // console.log(res.data);
        setMovies(res.data)
      }
      catch (err) {
        console.error("Failed to fetch movies:", err)
      }
    }

    fetchMedia()
  }, [])

  const [isLoggedIn, setIsLoggedIn] = useState(false);
  useEffect(() => {
    function CheckLogin() {
      const authCookie = document.cookie.split(';').some((cookie) => cookie.trim().startsWith('IMDB_Cookie='));
      setIsLoggedIn(authCookie);
    }

    CheckLogin();
  }, []);

  const [watchlistDict, setWatchlistDict] = useState({});
  const [watchlistRefreshKey, setWatchlistRefreshKey] = useState(0);
  useEffect(() => {
    async function fetchWatchlist() {
      try {
        const res = await axios.get("http://localhost:8080/api/watchlist/details", {
          withCredentials: true,
          headers: {
            'Content-Type': 'application/json',
          },
        });

        const watchListDict = res.data.media.reduce((dict, item) => {
          if (!dict[item.type]) {
            dict[item.type] = {};
          }
          dict[item.type][item.id] = item.title;
          return dict;
        }, {});

        console.log(watchListDict)

        setWatchlistDict(watchListDict)
      }
      catch (err) {
        console.error("Failed to fetch watchlist:", err)
        setWatchlistDict({})
      }
    }

    if (isLoggedIn)
      fetchWatchlist()
  }, [watchlistRefreshKey, isLoggedIn])

  const handleRefresh = async (type, id) => {
    console.log(watchlistDict)
    if (watchlistDict[type] && watchlistDict[type][id]) {
      try {
        // Manually assemble request because axios delete does not allow sending bodies?
        const res = await axios.request({
          "url": "http://localhost:8080/api/watchlist",
          "method": "DELETE",
          "data": {
            "Type": type,
            "Id": Number(id)
          },
          withCredentials: true,
          headers: {
            'Content-Type': 'application/json'
          }
        })
        setWatchlistRefreshKey(p => p + 1);
      }
      catch (err) {
        console.error("Failed to remove from watchlist:", err)
      }
    }
    else {
      try {
        const res = await axios.request({
          "url": "http://localhost:8080/api/watchlist",
          "method": "POST",
          "data": {
            "Type": type,
            "Id": Number(id)
          },
          withCredentials: true,
          headers: {
            'Content-Type': 'application/json'
          }
        })
        setWatchlistRefreshKey(p => p + 1);
      }
      catch (err) {
        console.error("Failed to add to watchlist:", err)
      }
    }
  };



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


  return (


    <>
      <Genres selectedGenres={selectedGenres} onToggleGenre={handleGenreToggle}></Genres>
      <MediaDisplayTable selectedGenres={selectedGenres} media={movies} watchlistDict={watchlistDict} handleWatclistChange={handleRefresh}></MediaDisplayTable>
    </>
  )
}

export default Movies