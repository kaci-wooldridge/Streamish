import React, { useState } from 'react';
import { useNavigate } from "react-router-dom";
import { Button, Form, FormGroup, Label, Input, FormText } from 'reactstrap';
import { addVideo } from "../modules/videoManager";

const VideoForm = ({ getVideos }) => {
    const emptyVideo = {
        title: '',
        description: '',
        url: ''
    };

    const [video, setVideo] = useState(emptyVideo);
    const navigate = useNavigate();

    const handleInputChange = (evt) => {
        const value = evt.target.value;
        const key = evt.target.id;

        const videoCopy = { ...video };

        videoCopy[key] = value;
        setVideo(videoCopy);
    };

    const handleSave = (evt) => {
        evt.preventDefault();
        addVideo(video).then(() => {
            navigate("/");
        });
    };

    return (
        <Form>
            <FormGroup>
                <Label for="title">Title</Label>
                <Input type="text" name="title" id="title" placeholder="video title"
                    value={video.title}
                    onChange={handleInputChange} />
            </FormGroup>
            <FormGroup>
                <Label for="url">URL</Label>
                <Input type="text" name="url" id="url" placeholder="video link"
                    value={video.url}
                    onChange={handleInputChange} />
            </FormGroup>
            <FormGroup>
                <Label for="description">Description</Label>
                <Input type="textarea" name="description" id="description"
                    value={video.description}
                    onChange={handleInputChange} />
            </FormGroup>
            <Button className="btn btn-primary" onClick={handleSave}>Submit</Button>
        </Form>
    );
};

export default VideoForm;