import React, { useEffect, useState } from 'react';
import { Box, CircularProgress } from '@mui/material';
import SinglePost from '../widget/SinglePost';
import { GetPostsByMuni, GetFormalPostsByMuni } from '../../utils/api';
import { useSelector } from 'react-redux';

const PostsSection = ({ type, municipalityId }) => {
  const [posts, setPosts] = useState([]);
  const [loading, setLoading] = useState(true);
  const token = useSelector((state) => state.user.accessToken);

  useEffect(() => {
    const fetchPosts = async () => {
        setLoading(true);
        if(type===0){
          const data = await GetPostsByMuni(token, municipalityId)
          setPosts(data);
        }else{
          const data = await GetFormalPostsByMuni(token, municipalityId)
          setPosts(data);
        }
        setLoading(false);
      };

    // Only fetch posts if a municipality is selected
    if (municipalityId) {
      fetchPosts();
    }
  }, [type, municipalityId]);

  return (
    <Box p={2} justifyContent="center">
      {loading ? (
        <Box display="flex"><CircularProgress sx={{m:"0 auto"}} /></Box>
      ) : (
        posts.map(
          ({
            UserId,
            Id,
            IssueId,
            PostType,
            Username,
            Text,
            ImageUrls,
            ProfilePictureUrl,
            UpVotesCount,
            Comments,
            IsUpVoted,
          }) => (
            <SinglePost
              key={Id}
              postId={Id}
              issueId={IssueId}
              postType={PostType}
              postUserId={UserId}
              username={Username}
              description={Text}
              picturePath={ImageUrls}
              profilePictureUrl={ProfilePictureUrl}
              upVoteCount={UpVotesCount}
              comments={Comments}
              isUpVoted={IsUpVoted}
            />
          )
        )
      )}
    </Box>
  );
};

export default PostsSection;