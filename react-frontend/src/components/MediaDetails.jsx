import { useParams, useLocation } from 'react-router-dom';
import { React, useEffect, useState } from 'react';
import Banner from '../images/banner.jpg';
import axios from 'axios';

function MediaDetails() {
    const { id } = useParams();
    const [movie, setMovie] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    const location = useLocation();
    const mediaType = location.state?.type;

    // TODO: List all actors in media
    // Use a different component for EACH person and just map them below (try to make it reusable for future-proofing about directors)

    useEffect(() => {

        async function fetchMovie() {
            try {
                let endpoint = mediaType === "Movie" ? "movies" : "shows"

                const res = await axios.get(`http://localhost:8080/api/media/${endpoint}/${id}`);
                // console.log(res.data);
                setMovie(res.data);
            }
            catch (err) {
                if (err.response?.status === 400)
                    setError(err.response.data);
                else
                    setError('An unexpected error occurred.');
            }

            setLoading(false)
        }

        fetchMovie();
    }, [id]);

    const formatDate = (dateStr) => {
        const date = new Date(dateStr);
        // I hate DateTime in React
        return date.toLocaleDateString('en-US', {
            year: 'numeric',
            month: 'long',
            day: 'numeric',
        });
    };

    if (loading) return <p className="p-4">Loading...</p>;
    if (error) return <p className="p-4 text-red-600">{error}</p>;

    // Move this to a shared function since i'm already using it at 5 different places
    function getImageUrl(posterImage) {
        // Ensure that a random NULL value won't cause issues
        if (posterImage && posterImage.length > 0) {
            const base64String = btoa(
                posterImage.reduce((data, byte) => data + String.fromCharCode(byte), '')
            )
            return `data:image/jpeg;base64,${base64String}`
        }
        else
            return Banner
    }

    return (
        <div className="bg-gradient-to-br from-gray-400 to-white min-h-screen py-12 px-6">
            <div className="w-[60vw] mx-auto bg-white rounded-2xl shadow-2xl overflow-hidden flex flex-col md:flex-row">
                <div className="p-8 flex-1">
                    <h1 className="text-4xl font-extrabold mb-4">{movie.title}</h1>
                    <p className="text-gray-700 text-lg mb-6">{movie.description}</p>

                    <div className="space-y-3 text-base">
                        <p><span className="font-semibold">Release Date:</span> {formatDate(movie.releaseDate)}</p>
                        <p><span className="font-semibold">Director:</span> {movie.director}</p>
                        <p><span className="font-semibold">Genres:</span> {movie.genres?.join(', ') || 'N/A'}</p>
                        <p><span className="font-semibold">Rating:</span> {movie.rating}</p>
                        <p><span className="font-semibold">Reviews:</span> {movie.reviews}</p>
                        <p><span className="font-semibold">Length:</span> {movie.length / 60} min</p>
                    </div>
                </div>
                <img
                    src={getImageUrl(movie.posterImage)}
                    alt={movie.title}
                    className="w-[35vw] h-[35vh] object-cover"
                />
            </div>
        </div>
    );


};

export default MediaDetails