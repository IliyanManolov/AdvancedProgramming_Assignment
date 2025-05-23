import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import FormInput from './inputs/FormInput';
import { useAuth } from './contexts/AuthContext';

function Login() {
    const [formData, setFormData] = useState({
        username: '',
        password: '',
    });

    const [error, setError] = useState('');
    const [userId, setUserId] = useState(null);
    const [loading, setLoading] = useState(false);
    const { isAuthenticated, setIsAuthenticated } = useAuth();
    const navigate = useNavigate();

    const handleChange = (e) => {
        setFormData((prev) => ({
            ...prev,
            [e.target.name]: e.target.value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setUserId(null);
        setLoading(true);

        try {
            const response = await axios.post(
                'http://localhost:8080/oauth/login',
                formData,
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': 'application/json',
                    },
                }
            );

            setUserId(response.data.id);

            setIsAuthenticated(true);
            // Redirect to home page after 2 seconds
            setTimeout(() => {
                navigate("/")
            }, 2000);
        }
        catch (err) {
            if (err.response?.status === 401) {
                setError(err.response.data);
            } else {
                setError('An unexpected error occurred.');
            }
        }

        setLoading(false);
    };

    return (
        <div className="max-w-md mx-auto mt-10 p-6 bg-white rounded-2xl shadow-lg">
            <h2 className="text-2xl font-bold mb-6 text-center">Login</h2>
            <form onSubmit={handleSubmit} className="space-y-4">
                <FormInput label="Username" name="username" value={formData.username} onChange={handleChange} required />
                <FormInput label="Password" name="password" type="password" value={formData.password} onChange={handleChange} required />

                {error && <p className="text-red-500">{error}</p>}
                {userId && <p className="text-green-600">Logged in successfully!</p>}

                <button
                    type="submit"
                    className="w-full bg-blue-600 text-white py-2 rounded-xl hover:bg-blue-700 transition"
                    disabled={loading}
                >
                    {loading ? 'Logging in...' : 'Login'}
                </button>
            </form>
        </div>
    );
};


export default Login;
