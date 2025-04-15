import { React } from 'react'
import ItemDispalyRow from './ItemDispalyRow';

function WatchlistDisplayTable({ mediaList, handleRefresh, watchlistDict }) {

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

                            {/* Intentionally empty */}
                            <th className="px-4 py-3 text-center"></th>

                        </tr>
                    </thead>
                    <tbody>
                        {mediaList.map((item, index) => (
                            <ItemDispalyRow item={item} index={index} handleRefresh={handleRefresh} watchlistDict={watchlistDict}></ItemDispalyRow>
                        ))}
                    </tbody>
                </table>
            </div>
        </>
    )
}

export default WatchlistDisplayTable