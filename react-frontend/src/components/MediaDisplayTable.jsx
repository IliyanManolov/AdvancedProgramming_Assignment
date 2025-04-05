import React from 'react'
import Banner from '../images/banner.jpg'

function MediaDisplayTable({selectedGenres, media}) {

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
            <th className="px-4 py-3 text-center">Rating</th>
            <th className="px-4 py-3 text-center">Reviews</th>
            <th className="px-4 py-3 text-center">Genres</th>
          </tr>
        </thead>
        <tbody>
          {filteredMedia.map((item, index) => (
            <tr key={item.id} className="border-t hover:bg-gray-50">
              <td className="px-4 py-3">
                <img
                  src={getImageUrl(item.posterImage)}
                  alt={item.title}
                  className="w-[10vw] h-[10vh] rounded object-cover"
                />
              </td>
              <td className="px-4 py-3 font-medium text-center">{item.title}</td>
              <td className="px-4 py-3 text-sm text-gray-600 text-center">
                {item.description}
              </td>
              <td className="px-4 py-3 text-center">{item.rating}</td>
              <td className="px-4 py-3 text-center">{item.reviews}</td>
              <td className="px-4 py-3 text-center">{item.genres.join(", ")}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
    </>
  )

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
}

export default MediaDisplayTable