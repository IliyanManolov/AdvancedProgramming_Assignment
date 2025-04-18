import React, { useState } from 'react'
import FormInput from '../inputs/FormInput'
import axios from 'axios';

function CreateGenre({ onSubmit }) {

    const [formData, setFormData] = useState({
        name: ''
    })

    const [error, setError] = useState('');
    const [userId, setGenreId] = useState(null);
    const [loading, setLoading] = useState(false);

    const handleChange = (e) => {
        setFormData((prev) => ({
            ...prev,
            [e.target.name]: e.target.type === 'number' ? Number(e.target.value) : e.target.value,
        }));
    };


    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setGenreId(null);
        setLoading(true);

        var copy = { ...formData }

        try {
            // console.log(copy);

            const response = await axios.post(
                'http://localhost:8080/api/genres/',
                copy,
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': 'application/json',
                    },
                }
            );
            setGenreId(response.data.id);
            onSubmit()
        }
        catch (err) {
            if (err.response?.status === 400) {
                console.log(err.response.data)
                setError(err.response.data);
            } else {
                setError('An unexpected error occurred.');
            }
        }

        setLoading(false);
    };

    return (
        <div className="w-[25vw] mx-auto mt-10 p-6 bg-white rounded-2xl shadow-lg">
            <h2 className="text-2xl font-bold mb-6 text-center">Create a genre</h2>
            <form onSubmit={handleSubmit} className="space-y-4">

                <FormInput label="Genre Name" name="name" value={formData.name} onChange={handleChange} required={true} />

                {error && <p className="text-red-500">{JSON.stringify(error)}</p>}
                {userId && <p className="text-green-600">Created genre successfully!</p>}

                <button
                    type="submit"
                    className="w-full bg-blue-600 text-white py-2 rounded-xl hover:bg-blue-700 transition"
                    disabled={loading}
                >
                    {loading ? 'Creating genre...' : 'Create genre'}
                </button>
            </form>
        </div>
    )
}

export default CreateGenre