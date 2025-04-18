import { createContext, useState, useContext } from "react";
import { useAuth } from "./AuthContext";
import axios from "axios";

const ReviewContext = createContext();

export const ReviewProvider = ({ children }) => {

    const { isAuthenticated } = useAuth();

    // Nested (2D) dictionary [type][id]: [{}, {}...] 
    const [reviewsDict, setReviewsDict] = useState({});

    const getReviews = async (id, type) => {
        if (reviewsDict[type] && reviewsDict[type][id])
            return reviewsDict[type][id];

        try {
            const res = await axios.request({
                "url": "http://localhost:8080/api/reviews/get",
                "method": "POST",
                "data": {
                    "MediaType": type,
                    "MediaId": Number(id)
                },
                withCredentials: true,
                headers: {
                    'Content-Type': 'application/json'
                }
            })

            // Update only the path we care about (save on API calls)
            setReviewsDict(prev => ({
                ...prev,
                [type]: {
                    ...prev[type],
                    [id]: res.data
                }
            }))

            // Return the response data instead of accessing the dictionary in case it hasnt updated yet
            return res.data;
        }
        catch (err) {
            console.error("Failed to fetch reviews for media:", err)
            return [];
        }
    }

    const addReview = async (formData) => {

        // TODO: potentially redirect to the Login page?
        // No point in wasting API calls if we are not logged in
        if (!isAuthenticated)
            return;

        try {
            await axios.post(
                'http://localhost:8080/api/reviews',
                formData,
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': 'application/json',
                    },
                }
            );

            // Move this to a separate function
            const refreshResponse = await axios.request({
                "url": "http://localhost:8080/api/reviews/get",
                "method": "POST",
                "data": {
                    "MediaType": formData.type,
                    "MediaId": Number(formData.id)
                },
                withCredentials: true,
                headers: {
                    'Content-Type': 'application/json'
                }
            })

            // Update only the path we care about (save on API calls)
            setReviewsDict(prev => ({
                ...prev,
                [formData.type]: {
                    ...prev[formData.type],
                    [formData.id]: refreshResponse.data
                }
            }))
        }
        catch (err) {
            console.error("Failed to add new review:", err);
        }
    }

    // IDEA: store reviews in nested dictionaries [mediaType][mediaId]
    // Whenever a new review is created refresh ONLY that specific part of the dictionary
    // Any time a new media is opened in details load the reviews for it

    return (
        <ReviewContext.Provider value={{ getReviews, addReview }}>
            {children}
        </ReviewContext.Provider>
    )
}

export const useReviews = () => useContext(ReviewContext);
