export default function formatDate(dateStr) {
    const date = new Date(dateStr);
    // I hate DateTime in React
    return date.toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
    });
};