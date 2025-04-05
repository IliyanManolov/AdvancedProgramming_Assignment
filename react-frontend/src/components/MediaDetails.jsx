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
        <div className="max-w-4xl mx-auto mt-10 p-6 bg-white rounded-2xl shadow-lg">
            <div className="flex flex-col md:flex-row gap-6">
                <img
                    src={getImageUrl(movie.posterImage)}
                    alt={movie.title}
                    className="w-full md:w-[300px] rounded-xl shadow object-cover"
                />

                <div className="flex-1">
                    <h1 className="text-3xl font-bold mb-2">{movie.title}</h1>
                    <p className="text-gray-600 mb-4">{movie.description}</p>

                    <ul className="space-y-2 text-sm text-gray-800">
                        <li><strong>Director:</strong> {movie.director}</li>
                        <li><strong>Release Date:</strong> {formatDate(movie.releaseDate)}</li>
                        {/* Need to think if a media can even have 0 genres? */}
                        <li><strong>Genres:</strong> {movie.genres?.length > 0 ? movie.genres.join(', ') : 'N/A'}</li>
                        <li><strong>Rating:</strong> {movie.rating ?? 'N/A'}</li>
                        <li><strong>Reviews:</strong> {movie.reviews}</li>
                        <li><strong>Length:</strong> {movie.length / 60} minutes</li>
                    </ul>
                </div>
            </div>
        </div>
    );


};

export default MediaDetails