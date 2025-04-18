import React from 'react'
import formatDate from '../Utils/FormatDate';

function ReviewsDisplay({ reviews }) {
    return (
        <div className="w-[70vw] mx-auto bg-white rounded-2xl shadow-xl p-6 mt-8">
            <h2 className="text-2xl font-bold mb-4">Reviews</h2>
            <div className="space-y-4 max-h-[400px] overflow-y-auto">
                {reviews?.length > 0 ? (
                    reviews.map((review, index) => (
                        <div key={index} className="border-b pb-4">
                            <div className="flex justify-between items-center mb-1">
                                <span className="font-semibold">{review.userName}</span>
                                <span className="text-sm text-gray-500">{formatDate(review.createdDate)}</span>
                            </div>
                            <p className="text-sm text-gray-600 mb-1">
                                <span className="font-semibold">Rating:</span>{review.rating}/10
                            </p>
                            <p className="text-gray-800">{review.reviewText}</p>
                        </div>
                    ))
                ) : (
                    <p className="text-gray-500">No reviews. Be the first to review</p>
                )}
            </div>
        </div>
    );
}

export default ReviewsDisplay