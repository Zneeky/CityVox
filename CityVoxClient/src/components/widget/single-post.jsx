import React from 'react';
import { Box, Button, InputBase, Divider, Card, CardContent, Avatar, Typography, CardMedia, CardActions, IconButton, SvgIcon, useTheme } from '@mui/material';
import ChevronDoubleUpIcon from '@heroicons/react/24/outline/ChevronDoubleUpIcon';
import ChatBubbleOvalLeftEllipsisIcon from '@heroicons/react/24/outline/ChatBubbleOvalLeftEllipsisIcon';
import { Carousel } from 'react-responsive-carousel';
import 'react-responsive-carousel/lib/styles/carousel.min.css';
import { Link } from 'react-router-dom';
import { useSelector } from 'react-redux/es/hooks/useSelector';
import { CreateComment, CreateUpVote, DeleteUpVote } from '../../utils/api';
import { useState } from 'react';
import ArrowTopRightOnSquareIcon from "@heroicons/react/24/solid/ArrowTopRightOnSquareIcon";
import { useNavigate } from 'react-router-dom';

const SinglePost = ({
  postId,
  postUserId,
  issueId,
  postType,
  username,
  description,
  picturePath,
  profilePictureUrl,
  upVoteCount,
  comments,
  isUpVoted,
}) => {

  const appUser = useSelector((state) => state.user);

  const navigate = useNavigate()
  const [voteState, setVoteState] = useState(isUpVoted);
  const [commentsState, setComments] = useState(comments.$values)
  const [upVoteCountState, setUpVoteCount] = useState(upVoteCount);
  const [isComments, setIsComments] = useState(false);
  const [newComment, setNewComment] = useState("");
  const images = picturePath ? picturePath.split(';') : [];

  const { palette } = useTheme();


  function mapPostTypeToPath(postType) {
    switch (postType) {
      case "Report":
        return "reports";
      case "Emergency":
        return "emergencies";
      case "Event":
        return "events";
      case "InfrastructureIssue":
        return "infrastructure_issues";
      default:
        return ""; // Handle other cases or errors
    }
  }

  const handleInputBaseKeyDown = async (event) => {
    if (event.keyCode === 13 && newComment.trim() !== "") {
      // Submit the input value
      const createPostDto = {
        Text: newComment,
        UserId: appUser.id,
        PostId: postId
      }
      setNewComment("");
      const response = await CreateComment(createPostDto);
      setComments(response);
    }
  };

  const handleLikePost = async (postId, voteState) => {
    try {
      if (!voteState) {
        const response = await CreateUpVote(postId);
        if (response.status === 200) { // assuming HTTP 200 means success
          setVoteState(!voteState);
          setUpVoteCount(upVoteCountState + 1); // assuming you have upVoteCount state
        }
      } else {
        const response = await DeleteUpVote(postId);
        if (response.status === 200) {
          setVoteState(!voteState);
          setUpVoteCount(upVoteCountState - 1); // assuming you have upVoteCount state
        }
      }
    } catch (err) {
      console.error(err);
      // handle error, maybe with a user-facing message
    }
  }


  return (
    <Card sx={{ mb: "1rem" }}>
      <CardContent sx={{ display: 'flex', alignItems: 'center' }}>
        <Link to={`/profile/${username}`} style={{ textDecoration: 'none', display: 'flex', alignItems: 'center' }}>
          <Avatar src={profilePictureUrl} />
          <Typography variant="h5" sx={{ marginLeft: '1rem', color: 'text.primary' }}>{username}</Typography>
        </Link>
        {postType && <Button
          onClick={() => navigate(`/${mapPostTypeToPath(postType)}/${issueId}`)}
          endIcon={
            <SvgIcon fontSize="small">
              <ArrowTopRightOnSquareIcon />
            </SvgIcon>
          }
          sx={{ ml: "2rem" }}
          variant="contained"
        >
          {postType}
        </Button>}
      </CardContent>
      <CardContent>
        <Typography variant="body1">{description}</Typography>
      </CardContent>

      <Carousel
        showThumbs={false}
        showArrows={false}
        showStatus={false}
        showIndicators={true}
        renderIndicator={(onClickHandler, isSelected, index, label) => {
          const commonStyle = {
            width: '10px',
            height: '10px',
            borderRadius: '50%',
            display: 'inline-block',
            margin: '0 4px',
            cursor: 'pointer',
          };
          if (isSelected) {
            return (
              <li
                style={{ ...commonStyle, background: 'rgba(0, 0, 0, 0.8)' }}
                aria-label={`Selected: ${label} ${index + 1}`}
                title={`Selected: ${label} ${index + 1}`}
              />
            );
          }
          return (
            <li
              style={{ ...commonStyle, background: 'rgba(204, 204, 204, 0.5)' }}
              onClick={onClickHandler}
              onKeyDown={onClickHandler}
              value={index}
              key={index}
              role="button"
              tabIndex={0}
              title={`${label} ${index + 1}`}
              aria-label={`${label} ${index + 1}`}
            />
          );
        }}
      >
        {images.map((image, index) => (
          <CardMedia
            key={index}
            component="img"
            alt="Post Image"
            height="600"
            image={image}
            title="Post Image"
          />
        ))}
      </Carousel>



      <CardActions >
        <IconButton
          aria-label="upVote"
          onClick={() => handleLikePost(postId, voteState)}
        >
          <SvgIcon>
            <ChevronDoubleUpIcon style={{ color: voteState ? 'orange' : 'gray' }} />
          </SvgIcon>
        </IconButton>
        <Typography>{upVoteCountState}</Typography>
        <IconButton
          aria-label="comment"
          onClick={() => setIsComments(!isComments)}
        >
          <SvgIcon>
            <ChatBubbleOvalLeftEllipsisIcon style={{ color: 'grey' }} />
          </SvgIcon>
        </IconButton>
        <Typography>{comments.length}</Typography>
      </CardActions>


      {isComments && (
        <Box mt="0.5rem">
          <Divider />
          <InputBase
            placeholder={"comment..."}
            multiline
            sx={{
              height: "6vh",
              width: "100%",
              transition: "rows 0.2s",
              overflowWrap: "break-word", // Enable text wrapping
              wordWrap: "break-word", // Alternative property for older browsers
              padding: "1rem 2rem",
              overflowX: "hidden",
              backgroundColor: palette.primary.light, // Set a maximum height
              "&::-webkit-scrollbar": {
                width: "0.4em",
              },
              "&::-webkit-scrollbar-track": {
                borderRadius: "8px",
                backgroundColor: "transparent",
              },
              "&::-webkit-scrollbar-thumb": {
                backgroundColor: palette.primary.main,
                borderRadius: "8px",
                border: "none",
                "&:hover": {
                  backgroundColor: palette.primary.dark,
                },
              },
            }}
            onKeyDown={handleInputBaseKeyDown}
            onChange={(e) => setNewComment(e.target.value)}
            value={newComment}
          />
          <Divider sx={{ borderColor: "orange", borderWidth: "1px" }} />
          <Box
            height="290px"
            pl="25px"
            overflow="auto"
            sx={{
              "&::-webkit-scrollbar": {
                width: "0.4em",
              },
              "&::-webkit-scrollbar-track": {
                borderRadius: "8px",
                backgroundColor: "transparent",
              },
              "&::-webkit-scrollbar-thumb": {
                backgroundColor: palette.primary.main,
                borderRadius: "8px",
                border: "none",
                "&:hover": {
                  backgroundColor: palette.primary.dark,
                },
              }, // For Internet Explorer and Edge
            }}
          >
            {commentsState.map((comment) => (
              <Box
                key={comment.Id}
                mt="6px"
                mr="10px"
                sx={{
                  display: "flex",
                  flexDirection: "row",
                  justifyContent: "space-between",
                }}
              >
                <Box
                  sx={{
                    display: "flex",
                    alignItems: "flex-start",
                    marginBottom: "1rem",
                  }}
                  gap="1rem"
                >
                  <Avatar src={comment.ProfilePictureUrl} size="35px" />
                  <Box>
                    <Box
                      sx={{
                        display: "flex",
                      }}
                      gap="0.5rem"
                    >
                      <Typography sx={{ fontWeight: "bold" }}>
                        {comment.Username}
                      </Typography>
                      <Typography>{comment.Text}</Typography>
                    </Box>
                    <Box mt="5px">
                      <Box
                        sx={{
                          display: "flex",
                        }}
                        gap="1rem"
                      >
                        <Typography
                          sx={{ fontSize: "0.75rem", color: "#8E8E8E" }}
                        >
                          {`${comment.CreatedAt}`}
                        </Typography>
                        {/* <Typography
                          sx={{ fontSize: "0.75rem", color: "#8E8E8E" }}
                        >
                          {comment.Likes ? comment.Likes.length : 0} likes
                        </Typography>
                        <Typography
                          sx={{ fontSize: "0.75rem", color: "#8E8E8E" }}
                        >
                          {comment.Replies ? comment.Replies.length : 0} replies
                        </Typography> */}
                      </Box>
                    </Box>
                  </Box>
                </Box>
                {/* <FavoriteBorderOutlined sx={{ fontSize: "15px" }} /> */}
              </Box>
            ))}
          </Box>
        </Box>
      )}
    </Card>
  );
};


export default SinglePost;