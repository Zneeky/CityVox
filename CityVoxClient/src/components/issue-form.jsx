// Import necessary React components and libraries
import React from "react";
import {
  Box,
  SvgIcon,
  Button,
  TextField,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Typography,
  useTheme,
} from "@mui/material";
import { Formik } from "formik";
import FlexBetween from "./styling/flex-between";
import * as Yup from "yup";
import PencilIcon from "@heroicons/react/24/outline/PencilIcon";
import Dropzone from "react-dropzone";

// Define validation schema for Yup to enforce form field constraints
const validationSchema = Yup.object({
  issueType: Yup.string().required("Issue type is required"),
  title: Yup.string()
    .required("Title is required")
    .min(4, "Title must be at least 4 characters")
    .max(50, "Title must be 50 characters or less"),
  description: Yup.string()
    .required("Description is required")
    .min(10, "Description must be at least 10 characters")
    .max(300, "Description must be 300 characters or less"),
  latitude: Yup.number().required("Latitude is required"),
  longitude: Yup.number().required("Longitude is required"),
});

// Functional component for the issue form
const IssueForm = ({ onFormSubmit, issueTypes, formData }) => {
  // Access the theme object from MUI for consistent styling
  const { palette } = useTheme();
  // Default values for form fields when the form is reset
  const emptyValues = {
    latitude: "",
    longitude: "",
    address: "",
    imageUrl: "",
    issueType: "",
    title: "",
    description: "",
  };

  return (
    // Formik wrapper manages the form state, validation, and submission
    <Formik
      initialValues={formData} // Initial values for form fields
      validationSchema={validationSchema} // Yup validation schema for form fields
      onSubmit={(values, { resetForm }) => {
        onFormSubmit(values); // Callback function to handle form submission
        resetForm(); // Resetting form fields after successful submission
      }}
      enableReinitialize // Allow reinitialization of form values
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
        // HTML form element with React handlers
        <form onSubmit={handleSubmit}>
          {/* Dropdown for selecting the type of the issue */}
          <FormControl fullWidth variant="filled">
            <InputLabel id="issueType-label">Issue Type</InputLabel>
            <Select
              labelId="issueType-label"
              name="issueType"
              value={values.issueType}
              onChange={handleChange}
              onBlur={handleBlur}
            >
              {issueTypes.map((type) => (
                // Menu items for each issue type
                <MenuItem key={type.value} value={type.value}>
                  {type.name}
                </MenuItem>
              ))}
            </Select>
            {/* Display an error message if the issue type is not selected */}
            {touched.issueType && errors.issueType && (
              <Typography sx={{ color: "red", fontSize: "small", ml: "1em" }}>
                {errors.issueType}
              </Typography>
            )}
          </FormControl>

          {/* Textfield for entering the title of the issue */}
          <TextField
            name="title"
            fullWidth
            margin="normal"
            label="Title"
            value={values.title}
            onChange={handleChange}
            onBlur={handleBlur}
            error={Boolean(touched.title && errors.title)}
            helperText={touched.title && errors.title}
          />

          {/* Hidden fields for storing latitude and longitude */}
          <input type="hidden" name="latitude" value={values.latitude} />
          <input type="hidden" name="longitude" value={values.longitude} />

          {/* Disabled textfield for displaying the address of the issue */}
          <TextField
            fullWidth
            disabled
            margin="normal"
            name="address"
            label="Address"
            value={values.address}
          />

          {/* Textfield for entering the description of the issue */}
          <TextField
            name="description"
            fullWidth
            multiline
            rows={4}
            margin="normal"
            label="Description"
            value={values.description}
            onChange={handleChange}
            onBlur={handleBlur}
            error={Boolean(touched.description && errors.description)}
            helperText={touched.description && errors.description}
          />

          {/* Dropzone component for uploading an image of the issue */}
          <Box
            gridColumn="span 4"
            border={`1px dashed ${palette.static.medium}`}
            borderRadius="5px"
            p="1rem"
          >
            {/* Dropzone functionality to handle image uploads */}
            <Dropzone
              acceptedFiles=".jpg,.jpeg,.png"
              multiple={false}
              onDrop={(acceptedFiles) =>
                setFieldValue("imageUrl", acceptedFiles[0])
              }
            >
              {({ getRootProps, getInputProps }) => (
                <Box
                  {...getRootProps()}
                  border={`2px dashed ${palette.primary.main}`}
                  p="1rem"
                  sx={{ "&:hover": { cursor: "pointer" } }}
                >
                  <input {...getInputProps()} />
                  {!values.imageUrl ? (
                    <p>Add a picture of the issue here</p>
                  ) : (
                    // Display the selected image file name with an edit icon
                    <FlexBetween>
                      <Typography>{values.imageUrl.name}</Typography>
                      <SvgIcon>
                        {" "}
                        <PencilIcon />
                      </SvgIcon>
                    </FlexBetween>
                  )}
                </Box>
              )}
            </Dropzone>
          </Box>

          {/* Submit button for submitting the form */}
          <Button type="submit" variant="contained" sx={{ mt: "4.5em" }}>
            Submit
          </Button>
        </form>
      )}
    </Formik>
  );
};

// Export the IssueForm component as the default export
export default IssueForm;
