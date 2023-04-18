import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import VideoList from "./VideoList";
import VideoForm from "./VideoForm";
import VideoDetails from "./VideoDetails";
import UserVideos from "./UserVideos";
import Login from "./Login";
import Register from "./Register";

const ApplicationViews = ({ isLoggedIn }) => {
    return (
        <Routes>
            <Route path="/" >
                <Route index element={
                    isLoggedIn
                        ?
                        <VideoList />
                        :
                        <Navigate to="/login" />
                    } />
                <Route path="videos">
                    <Route index element={<VideoList />} />
                    <Route path="add" element={<VideoForm />} />
                    <Route path=":id" element={<VideoDetails />} />
                </Route>
                <Route path="users">
                    <Route path =":id" element={<UserVideos />} />
                </Route>
            </Route>
            <Route path="login" element={<Login />} />
            <Route path="register" element={<Register />} />
            <Route path="*" element={<p>Whoops, nothing here...</p>} />
        </Routes>
    );
};

export default ApplicationViews;