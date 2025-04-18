import Banner from "../images/banner.jpg"

export default function getImageUrl(posterImage) {
  // Ensure that a random NULL value won't cause issues
  if (posterImage && typeof posterImage === 'string' && posterImage.length > 0) {
    return `data:image/jpeg;base64,${posterImage}`;
  }
  else
    return Banner
}