import { useParams, useLocation } from 'react-router-dom';
import { React, useEffect, useState } from 'react';
import axios from 'axios';
import getImageUrl from '../Utils/GetImageUrl'
import formatDate from '../Utils/FormatDate';
import { useReviews } from './contexts/ReviewContext';
import ReviewsDisplay from './ReviewsDisplay';
import ReviewForm from './ReviewForm';

function MediaDetails() {
    const { id } = useParams();
    const [media, setMedia] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    const [reviews, setReviews] = useState([]);
    const { getReviews, addReview } = useReviews();

    const location = useLocation();
    const mediaType = location.state?.type;

    const isShow = mediaType !== "Movie"
    // TODO: List all actors in media
    // Use a different component for EACH person and just map them below (try to make it reusable for future-proofing about directors)

    useEffect(() => {

        async function fetchMovie() {
            try {
                let endpoint = mediaType === "Movie" ? "movies" : "shows"

                const res = await axios.get(`http://localhost:8080/api/media/${endpoint}/${id}`);
                // console.log(res.data);
                setMedia(res.data);

                const reviewsData = await getReviews(id, mediaType);
                setReviews(reviewsData);
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

    const handleNewReview = async (form) => {

        form.mediaType = mediaType;
        form.mediaId = id;

        console.log(form);
        await addReview(form);
    }

    if (loading) return <p className="p-4">Loading...</p>;
    if (error) return <p className="p-4 text-red-600">{error}</p>;

    return (
        <div className="bg-gradient-to-br from-gray-400 to-white min-h-screen py-12 px-6">
            <div className="w-[60vw] mx-auto bg-white rounded-2xl shadow-2xl overflow-hidden flex flex-col md:flex-row">
                <div className="p-8 flex-1">
                    <h1 className="text-4xl font-extrabold mb-4">{media.title}</h1>
                    <p className="text-gray-700 text-lg mb-6">{media.description}</p>

                    <div className="space-y-3 text-base">
                        <p><span className="font-semibold">Release Date:</span> {formatDate(media.releaseDate)}</p>
                        {isShow === true && (
                            <>
                                <p><span className="font-semibold">End Date:</span> {media.showEndDate ? formatDate(media.releaseDate) : "Ongoing"}</p>
                            </>
                        )}
                        <p><span className="font-semibold">Director:</span> {media.director}</p>
                        <p><span className="font-semibold">Genres:</span> {media.genres?.join(', ') || 'N/A'}</p>
                        <p><span className="font-semibold">Rating:</span> {media.rating}</p>
                        <p><span className="font-semibold">Reviews:</span> {media.reviews}</p>
                        <p><span className="font-semibold">Length:</span> {Math.round(media.length / 60)} min</p>
                    </div>
                </div>
                <img
                    src={getImageUrl(media.posterImage)}
                    alt={media.title}
                    className="w-[35vw] h-[35vh] object-cover"
                />
            </div>

            <ReviewForm onSubmitReview={handleNewReview}></ReviewForm>
            <ReviewsDisplay reviews={reviews}></ReviewsDisplay>
        </div>
    );


};

export default MediaDetails