import { React, useEffect, useState } from 'react'
import { Link } from 'react-router-dom';
import getImageUrl from '../Utils/GetImageUrl'
import { useAuth } from './AuthContext';

function ItemDispalyRow({ item, index, handleRefresh, watchlistDict }) {

    const { isAuthenticated: isLoggedIn } = useAuth();

    const [icon, setIcon] = useState();
    useEffect(() => {
        if (isLoggedIn) {
            // Exists in watchlist
            if (watchlistDict[item.type] && watchlistDict[item.type][item.id])
                setIcon("💖")
            else
                setIcon("🤍");
        }
    })

    return (
        <tr key={index} className="border-t hover:bg-gray-50">
            <td className="px-4 py-3">
                {/* TBD: keep as state or change to URL parameter. Currently hidden and cannot be bookmarked */}
                <Link to={`/${item.type === 'TvShow' ? 'shows' : 'movies'}/${item.id}`} state={{ type: item.type }}>
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

            <td className="px-4 py-3 text-center">{item.type === 'TvShow' ? item.showSeasonsCount : "N/A"}</td>
            <td className="px-4 py-3 text-center">{item.type === 'TvShow' ? item.showEpisodesCount : "N/A"}</td>
            <td className="px-4 py-3 text-center">
                {
                    isLoggedIn && (
                        <>
                            <button onClick={async () => await handleRefresh(item.type, item.id)} className="text-xl">
                                {icon}
                            </button>
                        </>
                    )
                }
            </td>
        </tr>
    )
}

export default ItemDispalyRow