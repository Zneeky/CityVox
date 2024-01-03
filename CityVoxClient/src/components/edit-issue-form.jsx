import React, { useState, useEffect } from "react";
import {
  Box,
  Grid,
  Button,
  TextField,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Typography,
  useTheme,
  SvgIcon,
} from "@mui/material";
import {
  UpdateEmergency,
  UpdateInfIssue,
  UpdateReport,
  DeleteReport,
  DeleteEmergency,
  DeleteInfIssue,
  ForwardReportToCallSofia
} from "../utils/api";
import { useSelector } from "react-redux/es/hooks/useSelector";
import * as Yup from "yup";
import { Formik, Form } from "formik";
import Dropzone from "react-dropzone";
import { useNavigate } from "react-router-dom";
import FlexBetween from "./styling/flex-between";
import PencilIcon from "@heroicons/react/24/outline/PencilIcon";
import { uploadToCloudinary } from "../utils/api";

const validationSchema = Yup.object({
  TypeValue: Yup.string().required("Issue type is required"),
  Title: Yup.string()
    .required("Title is required")
    .max(40, "Title must be 100 characters or less"),
  Description: Yup.string()
    .required("Description is required")
    .max(500, "Description must be 500 characters or less"),
  StatusValue: Yup.string().required("Status is required"),
  // Add other validations if needed
});

const EditIssueForm = ({ type, issueTypes, statusTypes, issue }) => {
  const navigate = useNavigate();
  const [issueFormData, setIssue] = useState({
    Id: "",
    Title: "",
    Description: "",
    Latitude: "",
    Longitude: "",
    Type: "",
    Status: "",
    TypeValue: "",
    StatusValue: "",
    Address: "",
    CreatorUsername: "",
    Municipality: "",
  });
  const { palette } = useTheme();
  const username = useSelector((state) => state.user.username);
  const role = useSelector((state) => state.user.role);

  useEffect(() => {
    if (issue != null) {
      setIssue({
        Id: issue.Id,
        Title: issue.Title,
        Description: issue.Description,
        Latitude: issue.Latitude,
        Longitude: issue.Longitude,
        ImageUrl: issue.ImageUrl,
        Type: issue.Type,
        Status: issue.Status,
        TypeValue: issue.TypeValue,
        StatusValue: issue.StatusValue,
        Address: issue.Address,
        CreatorUsername: issue.CreatorUsername,
        Municipality: issue.Municipality,
      });
    }
  }, [issue]);

  const handleDeleteIssue = async () => {
    if (window.confirm("Are you sure you want to delete this issue?")) {
      // You can use a more elegant confirm modal/dialog from your UI library if available.
      if (type === "report") {
        await DeleteReport(issueFormData.Id);
      } else if (type === "emergency") {
        await DeleteEmergency(issueFormData.Id);
      } else if (type === "infIssue") {
        await DeleteInfIssue(issueFormData.Id);
      }
      // Handle something after deletion, e.g. redirecting to another page or displaying a message.
      //navigate('/home');
      window.history.back();
    }
  };

  const handleForwardReportToCallSofia = async () =>{
    if (window.confirm("Are you sure you want to forward this report to callsofia.bg?")) {
      const response = await ForwardReportToCallSofia(issueFormData);
      if(response.status==="Ok"){
        window.alert("Success")
      }
    }
  }

  const handleFormSubmit = async (values) => {
    // If ImageUrl is a File object
    if (values.ImageUrl instanceof File) {
      const imageUrl = await uploadToCloudinary(values.ImageUrl);
      values.ImageUrl = imageUrl;
    }

    if (type === "report") {
      await UpdateReport(values);
    } else if (type === "emergency") {
      await UpdateEmergency(values);
    } else if (type === "infIssue") {
      await UpdateInfIssue(values);
    }
    // Do something after updating, e.g., show a success message or redirect
    //navigate('/home');
    window.history.back();
  };

  return (
    <Formik
      initialValues={issueFormData}
      validationSchema={validationSchema}
      onSubmit={handleFormSubmit}
      enableReinitialize
    >
      {({
        values,
        errors,
        touched,
        handleChange,
        handleBlur,
        setFieldValue,
      }) => (
        <Form>
          <FormControl fullWidth variant="filled">
            <InputLabel id="issueType-label">Issue Type</InputLabel>
            <Select
              labelId="issueType-label"
              name="TypeValue"
              value={values.TypeValue}
              onChange={handleChange}
              onBlur={handleBlur}
            >
              {issueTypes.map((type) => (
                <MenuItem key={type.value} value={type.value}>
                  {type.name}
                </MenuItem>
              ))}
            </Select>
            {touched.TypeValue && errors.TypeValue && (
              <Typography sx={{ color: "red", fontSize: "small", ml: "1em" }}>
                {errors.TypeValue}
              </Typography>
            )}
          </FormControl>
          {/* Add other fields, similar to IssueForm component */}
          <TextField
            name="Title"
            fullWidth
            margin="normal"
            label="Title"
            value={values.Title}
            onChange={handleChange}
            onBlur={handleBlur}
            error={Boolean(touched.Title && errors.Title)}
            helperText={touched.Title && errors.Title}
          />
          <TextField
            fullWidth
            disabled
            margin="normal"
            name="Address"
            label="Address"
            value={values.Address}
          />
          <TextField
            name="Description"
            fullWidth
            multiline
            rows={4}
            margin="normal"
            label="Description"
            value={values.Description}
            onChange={handleChange}
            onBlur={handleBlur}
            error={Boolean(touched.Description && errors.Description)}
            helperText={touched.Description && errors.Description}
          />
          <Box
            gridColumn="span 4"
            border={`1px dashed ${palette.static.medium}`}
            borderRadius="5px"
            p="1rem"
          >
            <Dropzone
              acceptedFiles=".jpg,.jpeg,.png"
              multiple={false}
              onDrop={(acceptedFiles) =>
                setFieldValue("ImageUrl", acceptedFiles[0])
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
                  {!values.ImageUrl ? (
                    <p>Add picture of the issue here</p>
                  ) : (
                    <FlexBetween>
                      <Typography>
                        {values.ImageUrl && values.ImageUrl.name
                          ? values.ImageUrl.name
                          : "change current photo"}
                      </Typography>
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
          {role === "Admin" ? (
            <FormControl fullWidth variant="filled">
              <InputLabel id="statusType-label" mt="1em">
                Status Type
              </InputLabel>
              <Select
                labelId="statusType-label"
                name="StatusValue" // use Formik's name prop
                value={values.StatusValue} // use Formik's values
                onChange={handleChange} // use Formik's handleChange
                onBlur={handleBlur} // use Formik's handleBlur if needed
              >
                {statusTypes.map((type) => (
                  <MenuItem key={type.value} value={type.value}>
                    {type.name}
                  </MenuItem>
                ))}
              </Select>
              {touched.StatusValue && errors.StatusValue && (
                <Typography sx={{ color: "red", fontSize: "small", ml: "1em" }}>
                  {errors.StatusValue}
                </Typography>
              )}
            </FormControl>
          ) : (
            <TextField
              fullWidth
              disabled
              margin="normal"
              name="StatusValue"
              label="status"
              value={values.StatusValue} // use Formik's values
              error={Boolean(touched.StatusValue && errors.StatusValue)} // use Formik's touched and errors
              helperText={touched.StatusValue && errors.StatusValue} // use Formik's touched and errors
            />
          )}
          <Grid container spacing={2} alignItems="center" justifyContent="space-between">
            <Grid item >
              <Button type="submit" variant="contained" sx={{ mt: "4.5em" }}>
                Edit
              </Button>
            </Grid>

            {(role === "Admin" || issueFormData.CreatorUsername === username) && (
              <Grid item xs>
                <Button
                  variant="contained"
                  color="secondary"
                  onClick={handleDeleteIssue}
                  sx={{ mt: "4.5em" }}
                >
                  Delete
                </Button>
              </Grid>
            )}

            {(role === "Admin" && type === "report") && (
              <Grid item xs>
                <Button
                  variant="contained"
                  color="background"
                  onClick={handleForwardReportToCallSofia}
                  sx={{ mt: "4.5em", width: '100%' }}
                >
                  Forward Report
                </Button>
              </Grid>
            )}
          </Grid>
        </Form>
      )}
    </Formik>
  );
};

export default EditIssueForm;
