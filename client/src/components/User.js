import React from "react";
import { Link } from "react-router-dom";
import { Card, CardBody } from "reactstrap";

const User = ({ video }) => {
    return (
        <Card >
            <CardBody className="card-body">
                <iframe className="video"
                    src={video.url}
                    title="YouTube video player"
                    frameBorder="0"
                    allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                    allowFullScreen />
            </CardBody>
            <div>
                <Link to={`/videos/${video.id}`}>
                    <h4><strong>{video.title}</strong></h4>
                </Link>
            </div>
            <p>{video.description}</p>
            {
                video.comments != ""
                    ?
                    <p>
                        <strong>Comments</strong>
                        <br>
                        </br>
                        {video.comments.map(c => c.message)}
                    </p>
                    :
                    ""
            }
        </Card>
    );
};

export default User;