import logo from './logo.svg';
import './App.css';
import NavigationBar from './components/NavigationBar';
import Banner from './components/Banner';
import MediaDiscovery from './components/MediaDiscovery';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Movies from './components/Movies';
import NotFound from './components/NotFound';

// Tailwind will be used for CSS

function App() {
  return (

    <>
      <BrowserRouter>

        <NavigationBar></NavigationBar>
        <Routes>
          <Route path="/" element={
            <>

              <Banner>
              </Banner>

              <MediaDiscovery>
              </MediaDiscovery>      
            </>
          }></Route>

          <Route path="/movies" element= {
            <>
              <Movies></Movies>
            </>
          }></Route>

          <Route path="*" element={
            <NotFound></NotFound>
          }></Route>

        </Routes>
      </BrowserRouter>
    </>

    
    // navbar
    // banner - TBD? - generic "movie" image covering the entire width
    // Movie / Series selection (inside navbar)
  );
}

export default App;
