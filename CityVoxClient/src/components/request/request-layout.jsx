import ArrowUpOnSquareIcon from '@heroicons/react/24/solid/ArrowUpOnSquareIcon';
import ArrowDownOnSquareIcon from '@heroicons/react/24/solid/ArrowDownOnSquareIcon';
import PlusIcon from '@heroicons/react/24/solid/PlusIcon';
import {
  Box,
  Button,
  Container,
  Pagination,
  Stack,
  SvgIcon,
  Typography,
  Unstable_Grid2 as Grid
} from '@mui/material';
import { RequestCard } from './request-card';
import { useState, useEffect } from 'react';
import { GetRequestedEmergencies, GetRequestedEmergenciesCount, GetRequestedReports, GetRequestedReportsCount, GetRequestedInfIssues, GetRequestedInfIssuesCount } from '../../utils/api';
import { useSelector } from 'react-redux/es/hooks/useSelector';

const RequestLayout = ({ type }) => {

  const [requests, setRequests] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const cardsPerPage = 6;

  const handlePageChange = (event, value) => {
    setCurrentPage(value);
  };

  useEffect(() => {
    //REPORTS
    const fetchRequestedReportsCount = async () => {
      const response = await GetRequestedReportsCount();
      if (!isNaN(response.data)) {
        setTotalPages(Math.ceil(response.data / cardsPerPage));
      }
    };

    const fetchRequestedReports = async () => {
      const response = await GetRequestedReports(currentPage - 1, cardsPerPage);

      setRequests(response);
    }

    //EMERGENCIES
    const fetchRequestedEmergeciesCount = async () => {
      const response = await GetRequestedEmergenciesCount();
      if (!isNaN(response)) {
        setTotalPages(Math.ceil(response.data / cardsPerPage));
      }
    };

    const fetchRequestedEmergencies = async () => {
      const response = await GetRequestedEmergencies(currentPage - 1, cardsPerPage);

      setRequests(response);
    }

    //InfIssues
    const fetchRequestedInfIssuesCount = async () => {
      const response = await GetRequestedInfIssuesCount();
      if (!isNaN(response)) {
        setTotalPages(Math.ceil(response.data / cardsPerPage));
      }
    };

    const fetchRequestedInfIssues = async () => {
      const response = await GetRequestedInfIssues(currentPage - 1, cardsPerPage);

      setRequests(response);
    }

    if (type === "reports") {
      fetchRequestedReportsCount();
      fetchRequestedReports();
    } else if (type === "emergencies") {
      fetchRequestedEmergeciesCount();
      fetchRequestedEmergencies();
    } else if (type === "infrastructure_issues") {
      fetchRequestedInfIssuesCount();
      fetchRequestedInfIssues();
    }
  }, [type, currentPage, totalPages])
  return (
    <Box
      component="main"
      sx={{
        flexGrow: 1,
        py: 8
      }}
    >
      <Container maxWidth="xl">
        <Stack spacing={3}>
          <Stack
            direction="row"
            justifyContent="space-between"
            spacing={4}
          >
            <Stack spacing={1}>
              <Typography variant="h4">
                Requests
              </Typography>
              <Stack
                alignItems="center"
                direction="row"
                spacing={1}
              >
                <Typography>
                  /{type}
                </Typography>
              </Stack>
            </Stack>
            {/* <div>
              <Button
                startIcon={(
                  <SvgIcon fontSize="small">
                    <PlusIcon />
                  </SvgIcon>
                )}
                variant="contained"
              >
                Add
              </Button>
            </div> */}
          </Stack>
          <Box
            sx={{
              display: 'flex',
              justifyContent: 'center'
            }}
          >
            <Pagination
              count={totalPages}
              page={currentPage}
              size="small"
              onChange={handlePageChange} 
            />
          </Box>
          <Grid
            container
            spacing={3}
          >
            {requests ? requests.map((request) => (
              <Grid
                xs={12}
                md={6}
                lg={4}
                key={request.Id}
              >
                <RequestCard request={request} type={type} />
              </Grid>
            )) : <Typography>No requests</Typography>}
          </Grid>
        </Stack>
      </Container>
    </Box>
  )
}

export default RequestLayout;