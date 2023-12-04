import { Box, Dialog, DialogTitle, DialogContent, FormControl, InputLabel, Select, MenuItem, Button, TextField } from '@mui/material';
import { useState } from 'react';
import { Formik, Form, Field } from 'formik';
import * as Yup from 'yup';
import RegionDropdown from '../dropdown/region-dropdown';
import MunicipalityDropdown from '../dropdown/municipality-dropdown';
import { PromoteToAdmin, PromoteToRepresentative } from '../../utils/api';
import { useSelector } from 'react-redux/es/hooks/useSelector';

const CustomTextField = ({ label, field, form: { touched, errors }, ...props }) => (
    <TextField
        sx={{ mt: "1rem", mr: "1rem" }}
        {...field}
        {...props}
        label={label}
        error={touched[field.name] && !!errors[field.name]}
        helperText={touched[field.name] && errors[field.name]}
    />
);


const UserRoleChangeDialog = ({ open, onClose, username }) => {
    const [role, setRole] = useState('');

    const [selectedRegionId, setSelectedRegionId] = useState(null);
    const [selectedMunicipality, setSelectedMunicipality] = useState(null);  // This will hold the selected municipality data

    const appUser = useSelector((state) => state.user)
    const RepresentativeFormSchema = Yup.object().shape({
        MunicipalityId: Yup.string().required("Required"),
        Position: Yup.string().required("Required").max(20),
        Department: Yup.string().required("Required").max(40),
        OfficePhoneNumber: Yup.string().max(10),
        OfficeEmail: Yup.string().max(320)
    });

    const handleRoleChange = async (values) => {
        if (role === "representative") {
            // API call to promote to representative using values
            if (window.confirm('Are you sure you want to promote this user to representative?')){
                const createMuniRepDto ={
                    MunicipalityId:values.MunicipalityId,
                    Username:username,
                    Position:values.Position,
                    Department:values.Department,
                    OfficePhoneNumber: values.OfficePhoneNumber,
                    OfficeEmail: values.OfficeEmail
                }

                const response = await PromoteToRepresentative(createMuniRepDto);
                console.log(response);
            }
        } else if (role === "Admin") {
            // API call to promote to admin
            if (window.confirm('Are you sure you want to promote this user to admin?')){
                const response = await PromoteToAdmin(username);
                console.log(response);
            }
        }
    };

    return (
        <Dialog open={open} onClose={onClose}>
            <DialogTitle>Change User Role</DialogTitle>
            <DialogContent>
                <FormControl fullWidth variant="filled" sx={{ mt: 1, maxWidth: "150px" }}>
                    <InputLabel id="demo-simple-select-label">Role</InputLabel>
                    <Select
                        labelId="demo-simple-select-label"
                        value={role}
                        onChange={(e) => setRole(e.target.value)}
                        label="Role"
                    >
                        <MenuItem value={"Admin"}>Admin</MenuItem>
                        <MenuItem value={"Representative"}>Representative</MenuItem>
                    </Select>
                </FormControl>

                {role === "Representative" && (
                    <>
                        <Box mt="1rem" display="flex">
                            <RegionDropdown onChange={setSelectedRegionId} />
                            {selectedRegionId &&
                                <MunicipalityDropdown
                                    regionId={selectedRegionId}
                                    onChange={(data) => setSelectedMunicipality(data)}
                                />}
                        </Box>
                        <Formik
                            initialValues={{
                                MunicipalityId: selectedMunicipality?.municipalityId || "",
                                Username: username,
                                Position: "",
                                Department: "",
                                OfficePhoneNumber: "",
                                OfficeEmail: ""
                            }}
                            validationSchema={RepresentativeFormSchema}
                            onSubmit={handleRoleChange}
                        >
                            {({ isSubmitting }) => (
                                <Form>
                                    <Field name="MunicipalityId" type="text" value={selectedMunicipality?.municipalityId || ""}  label="Municipality ID" component={CustomTextField} />
                                    <Field name="Position" type="text" label="Position" component={CustomTextField} />
                                    <Field name="Department" type="text" label="Department" component={CustomTextField} />
                                    <Field name="OfficePhoneNumber" type="text" label="Office Phone Number" component={CustomTextField} />
                                    <Field name="OfficeEmail" type="email" label="Office Email" component={CustomTextField} />

                                    <Button sx={{ mt: "1.7rem" }} type="submit" disabled={isSubmitting}>Submit</Button>
                                </Form>
                            )}
                        </Formik>
                    </>
                )}
                {role === "Admin" && (
                    <Button onClick={() => handleRoleChange()}>Confirm Promotion to Admin</Button>
                )}
            </DialogContent>
        </Dialog>
    );
}

export default UserRoleChangeDialog;