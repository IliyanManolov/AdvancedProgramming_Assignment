import React from 'react'
import { Link } from 'react-router-dom';
import getImageUrl from '../Utils/GetImageUrl'

function MediaDisplayTable({selectedGenres, media, mediaType}) {

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
            {mediaType === "shows" && (
                <>
                  <th className="px-4 py-3 text-center">Seasons</th>
                  <th className="px-4 py-3 text-center">Episodes</th>
                </>
              )}
          </tr>
        </thead>
        <tbody>
          {filteredMedia.map((item, index) => (
            <tr key={item.id} className="border-t hover:bg-gray-50">
              <td className="px-4 py-3">
                {/* TBD: keep as state or change to URL parameter. Currently hidden and cannot be bookmarked */}
                <Link to={`/${mediaType}/${item.id}`} state={{ type: item.type }}>
                  <img
                    src={getImageUrl(item.posterImage)}
                    alt={item.title}
                    className="w-[10vw] h-[10vh] rounded object-cover"
                  />
                </Link>
              </td>
              <td className="px-4 py-3 font-medium text-center">{item.title}</td>
              <td className="px-4 py-3 text-sm text-gray-600 text-center">
                {item.description}
              </td>
              <td className="px-4 py-3 text-center">{item.genres.join(", ")}</td>
              <td className="px-4 py-3 text-center">{item.rating}</td>
              <td className="px-4 py-3 text-center">{item.reviews}</td>
              {mediaType === "shows" && (
                <>
                  <td className="px-4 py-3 text-center">{item.showSeasonsCount}</td>
                  <td className="px-4 py-3 text-center">{item.showEpisodesCount}</td>
                </>
              )}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
    </>
  )
}

export default MediaDisplayTable