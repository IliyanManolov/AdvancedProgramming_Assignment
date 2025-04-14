import { React, useEffect, useState } from 'react';
import ItemDispalyRow from './ItemDispalyRow';


function MediaDisplayTable({ selectedGenres, media, watchlistDict, handleWatclistChange }) {

  const [isLoggedIn, setIsLoggedIn] = useState(false);
  useEffect(() => {
    function CheckLogin() {
      const authCookie = document.cookie.split(';').some((cookie) => cookie.trim().startsWith('IMDB_Cookie='));
      setIsLoggedIn(authCookie);
    }

    CheckLogin();
  }, []);

  // Annoying filtering because no LINQ...
  const filteredMedia = selectedGenres.length > 0
    ? media.filter(currentMedia =>
      currentMedia.genres.some(currentGenre => selectedGenres.includes(currentGenre))
    )
    : media;

  return (
    <>
      <div className="overflow-x-auto p-4">
        <table className="min-w-full table-auto border-collapse rounded-xl shadow-md">
          <thead className="bg-gray-100">
            <tr>
              <th className="px-4 py-3 w-[10vw] text-center"></th>
              <th className="px-4 py-3 text-center">Title</th>
              <th className="px-4 py-3 text-center">Description</th>
              <th className="px-4 py-3 text-center">Genres</th>
              <th className="px-4 py-3 text-center">Rating</th>
              <th className="px-4 py-3 text-center">Reviews</th>
              <th className="px-4 py-3 text-center">Seasons</th>
              <th className="px-4 py-3 text-center">Episodes</th>
              {
                // Intentionally empty
                isLoggedIn &&
                (
                  <>
                    <th className="px-4 py-3 text-center"></th>
                  </>
                )
              }
            </tr>
          </thead>
          <tbody>
            {filteredMedia.map((item, index) => (
              <ItemDispalyRow item={item} index={index} handleRefresh={handleWatclistChange} watchlistDict={watchlistDict}></ItemDispalyRow>
            ))}
          </tbody>
        </table>
      </div>
    </>
  )
}

export default MediaDisplayTable