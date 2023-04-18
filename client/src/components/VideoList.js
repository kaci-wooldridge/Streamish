import React, { useEffect, useState } from "react";
import Video from './Video';
import { getAllVideos, searchVideos } from "../modules/videoManager";

const VideoList = () => {
    const [videos, setVideos] = useState([]);
    const [searchTerm, setSearchTerm] = useState([]);

    const getVideos = () => {
        getAllVideos().then(videos => setVideos(videos));
    };

    const handleSearchSubmit = (event) => {
        event.preventDefault();
        searchVideos(searchTerm)
            .then(videos => setVideos(videos));
    }

    useEffect(() => {
        getVideos();
    }, []);

    return (
        <>
            <div className="container-top">
                <div className="search-container">
                    <form onSubmit={(event) => handleSearchSubmit(event)}>
                        <input
                            id="search-value"
                            type="text"
                            onChange={(event) => setSearchTerm(event.target.value)} />
                        <br></br>
                        <input
                            className="search-button"
                            id="search-submit"
                            type="submit"
                            value=" Search " />
                    </form>
                </div>
            </div>
            
            <div className="list-container">
                <div className="row justify-content-center">
                    {videos.map((video) => (
                        <Video video={video} key={video.id} />
                    ))}
                </div>
            </div>
        </>
    );
};

export default VideoList;