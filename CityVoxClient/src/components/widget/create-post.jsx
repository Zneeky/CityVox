import React, { useState, useEffect } from "react";
import {
  Avatar,
  Select,
  MenuItem,
  Card,
  Button,
  TextField,
  Box,
  Typography,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Divider,
  InputBase,
  useTheme,
} from "@mui/material";
import WidgetWrapper from "../styling/widget-wrapper";
import Dropzone from "react-dropzone";
import { useDispatch, useSelector } from "react-redux";
import * as Yup from "yup";
import { Formik, Field, Form } from "formik";
import FlexBetween from "../styling/flex-between";
import {
  EditOutlined,
  DeleteOutlined,
  AttachFileOutlined,
  GifBoxOutlined,
  ImageOutlined,
  MicOutlined,
  MoreHorizOutlined,
} from "@mui/icons-material";
import {
  GetApprovedIssuesForUser,
  uploadToCloudinary,
  CreatePost as CreatePostApi,
  CreateFormalPost as CreateFormalPostApi,
} from "../../utils/api";

const CreatePostSchema = Yup.object().shape({
  text: Yup.string().required("Text is required"),
});

const CreatePost = ({ open, handleClose }) => {
  const [userIssues, setUserIssues] = useState([]);
  const [postText, setPostText] = useState("");
  const [isImage, setIsImage] = useState(false);
  const [imageUrls, setImageUrls] = useState("");
  const user = useSelector((state) => state.user);
  const { palette } = useTheme();

  const getNumber = (represent) => {
    switch (represent) {
      case "report":
        return 0;
        break;
      case "emergency":
        return 1;
        break;
      case "infrastructure_issue":
        return 2;
        break;
      case "event":
        return 3;
        break;
      case "formal":
        return 4;
        break;
    }
  };

  const handlePostSubmit = async (values) => {
    if (user.role !== "representative") {
      // Implement logic here to send the post data to your backend
      let uploadedImageUrls = [];

      // Check if there are images and upload them to Cloudinary concurrently
      if (values.images && values.images.length > 0) {
        const uploadPromises = values.images.map((file) =>
          uploadToCloudinary(file)
        );

        try {
          // Await all image uploads at once
          uploadedImageUrls = await Promise.all(uploadPromises);
        } catch (error) {
          console.error("Error uploading one or more images:", error);
          // Handle the error as appropriate
          // You might want to inform the user that some images failed to upload
        }
      }

      // Construct the ImageUrls property
      const joinedImageUrls = uploadedImageUrls.join(";");
      const [selectedIssueId, selectedIssueType] =
        values.selectedIssue.split(",");

      const postData = {
        Text: values.text,
        ImageUrls: joinedImageUrls,
        PostType: Number(selectedIssueType), // You'll need to define how to determine the PostType.
        IssueId: selectedIssueId, // You'll need to define how to determine the IssueId.
        UserId: user.id, // Assuming you have the user object available from your state.
      };

      try {
        const result = await CreatePostApi(postData); // Assuming you have the user's accessToken from your state.
        console.log(result); // The result from your backend, e.g., "Post creation status: true"

        // After creating the post
        setPostText("");
        setImageUrls("");
        handleClose(); // Close the dialog after submitting
      } catch (error) {
        console.log(error);
      }
    } else {
      // Implement logic here to send the post data to your backend
      let uploadedImageUrls = [];

      // Check if there are images and upload them to Cloudinary concurrently
      if (values.images && values.images.length > 0) {
        const uploadPromises = values.images.map((file) =>
          uploadToCloudinary(file)
        );

        try {
          // Await all image uploads at once
          uploadedImageUrls = await Promise.all(uploadPromises);
        } catch (error) {
          console.error("Error uploading one or more images:", error);
          // Handle the error as appropriate
          // You might want to inform the user that some images failed to upload
        }
      }

      // Construct the ImageUrls property
      const joinedImageUrls = uploadedImageUrls.join(";");

      const postData = {
        Text: values.text,
        ImageUrls: joinedImageUrls,
        PostType: 4, // You'll need to define how to determine the PostType.
        UserId: user.id, // Assuming you have the user object available from your state.
      };

      try {
        const result = await CreateFormalPostApi(postData); // Assuming you have the user's accessToken from your state.
        console.log(result); // The result from your backend, e.g., "Post creation status: true"

        // After creating the post
        setPostText("");
        setImageUrls("");
        handleClose(); // Close the dialog after submitting
      } catch (error) {
        console.log(error);
      }
    }
  };

  useEffect(() => {
    const fetchUserIssues = async () => {
      const issues = await GetApprovedIssuesForUser(user.id);
      setUserIssues(issues || []);
    };

    if (user.role !== "Representative") {
      fetchUserIssues();
    }
  }, [user.id, user.role]);

  return (
    <Dialog
      open={open}
      onClose={handleClose}
      fullWidth // This makes the dialog use the full width
      maxWidth="md" // This sets the maximum width to the theme's medium breakpoint (you can adjust this)
      PaperProps={{
        // This is used to style the inner paper of the dialog
        style: {
          minHeight: "600px", // Set the minimum height of the dialog content
        },
      }}
    >
      <DialogTitle>Create a New Post</DialogTitle>
      <DialogContent sx={{ height: "100%", width: "100%" }}>
        {user.role === "representative" && (
          <Typography>For municipality representatives only</Typography>
        )}
        <Formik
          initialValues={{
            text: "",
            images: null,
            selectedIssue: "",
          }}
          validationSchema={CreatePostSchema}
          onSubmit={(values) => handlePostSubmit(values)}
        >
          {({
            values,
            errors,
            touched,
            handleChange,
            handleBlur,
            handleSubmit,
            setFieldValue,
          }) => (
            <Form>
              <WidgetWrapper>
                <FlexBetween gap="1.5rem">
                  <Avatar src={user.pfp} />
                  <InputBase
                    name="text"
                    placeholder={"Describe your issue here..."}
                    value={values.text}
                    onChange={handleChange}
                    multiline
                    style={{
                      width: "100%",
                      backgroundColor: palette.primary.light,
                      borderRadius: "8px",
                      padding: "1rem 2rem",
                      color: "black",
                    }}
                  />
                </FlexBetween>
                {/* Dropdown for user's approved issues (if they have any) */}
                {user.role !== "representative" &&
                  (userIssues.length > 0 ? (
                    <Box mb={2}>
                      <Select
                        value={values.selectedIssue}
                        onChange={(event) =>
                          setFieldValue("selectedIssue", event.target.value)
                        }
                        fullWidth
                        variant="outlined"
                      >
                        {userIssues.map((issue, index) => {
                          return (
                            <MenuItem
                              key={index}
                              value={`${issue.Id},${getNumber(
                                issue.Represent
                              )}`}
                            >
                              {issue.Title} - ID:{issue.Id} - Muni:
                              {issue.Municipality}
                            </MenuItem>
                          );
                        })}
                      </Select>
                    </Box>
                  ) : (
                    <Box mb={2} display="flex" justifyContent="center">
                      <Typography color="red" variant="body2">
                        You do not have any viable issues to post about
                      </Typography>
                    </Box>
                  ))}
                {isImage && (
                  <Box
                    border={`1px solid ${palette.primary.main}`}
                    borderRadius="5px"
                    mt="1rem"
                    p="1rem"
                  >
                    <Dropzone
                      acceptedFiles=".jpg,.jpeg,.png"
                      multiple={true}
                      onDrop={
                        (acceptedFiles) =>
                          setFieldValue("images", acceptedFiles) // Here we are setting the entire array of accepted files
                      }
                    >
                      {({ getRootProps, getInputProps }) => (
                        <FlexBetween>
                          <Box
                            {...getRootProps()}
                            border={`2px dashed ${palette.primary.main}`}
                            p="1rem"
                            width="100%"
                            sx={{ "&:hover": { cursor: "pointer" } }}
                            color={palette.primary.main}
                          >
                            <input {...getInputProps()} />
                            {!values.images ? (
                              <p>Add Images here</p>
                            ) : (
                              values.images.map((image, index) => (
                                <FlexBetween key={index}>
                                  <Typography>{image.name}</Typography>
                                  <EditOutlined />
                                </FlexBetween>
                              ))
                            )}
                          </Box>
                          {values.images && values.images.length > 0 && (
                            <DeleteOutlined
                              sx={{ color: palette.primary.main }}
                              onClick={() => setFieldValue("images", null)} // Clearing the entire array of images
                            />
                          )}
                        </FlexBetween>
                      )}
                    </Dropzone>
                  </Box>
                )}

                <Divider sx={{ margin: "1.25rem 0" }} />
                {/* Other content here */}
                <FlexBetween>
                  <FlexBetween
                    gap="0.25rem"
                    onClick={() => setIsImage(!isImage)}
                  >
                    <ImageOutlined sx={{ color: palette.primary.main }} />
                    <Typography
                      color={palette.primary.main}
                      sx={{
                        "&:hover": {
                          cursor: "pointer",
                          color: palette.primary.medium,
                        },
                      }}
                    >
                      Image
                    </Typography>
                  </FlexBetween>
                  {user.role === "representative" ? (
                    <Button
                      type="submit"
                      disabled={!values.text}
                      sx={{
                        color: palette.primary.light,
                        backgroundColor: palette.primary.main,
                        borderRadius: "3rem",
                        "&: hover": {
                          color: palette.primary.main,
                        },
                      }}
                    >
                      POST
                    </Button>
                  ) : (
                    <Button
                      type="submit"
                      disabled={!values.text || userIssues.length === 0}
                      sx={{
                        color: palette.primary.light,
                        backgroundColor: palette.primary.main,
                        borderRadius: "3rem",
                        "&: hover": {
                          color: palette.primary.main,
                        },
                      }}
                    >
                      POST
                    </Button>
                  )}
                </FlexBetween>
              </WidgetWrapper>
            </Form>
          )}
        </Formik>
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} color="primary">
          Cancel
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default CreatePost;
