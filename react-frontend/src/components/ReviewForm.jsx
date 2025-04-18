import { React, useState } from 'react'
import FormInput from './FormInput'
import SlideInput from './SlideInput';

function ReviewForm({ onSubmitReview }) {

    const [reviewText, setReviewText] = useState('');
    const [rating, setRating] = useState('');

    const handleSubmit = (e) => {
        e.preventDefault();

        // Avoid attempts to send empty values
        if (!reviewText || !rating)
            return;

        onSubmitReview(
            {
                review: reviewText,
                rating: parseFloat(rating)
            }
        );
        setReviewText('');
        setRating('');
    };
    return (
        <form onSubmit={handleSubmit} className="bg-white rounded-xl shadow-md p-6 w-[70vw] mb-8 mx-auto mt-8">
            <h2 className="text-xl font-bold mb-4 text-center">Write a Review</h2>

            <div className="space-y-4">
                <FormInput label="Your Review" name="reviewText" value={reviewText} onChange={(e) => setReviewText(e.target.value)} type="text" required={true} />
                <SlideInput label="Rating (0 - 10)" name="rating" value={rating} onChange={(e) => setRating(e.target.value)} type="number" stepRate={0.5} min={0} max={10} required={true}></SlideInput>
                <div className="flex justify-center">
                    <button type="submit" className="bg-blue-500 hover:bg-blue-600 text-white font-semibold px-4 py-2 rounded-lg transition">
                        Submit Review
                    </button>
                </div>
            </div>
        </form>
    )
}

export default ReviewForm