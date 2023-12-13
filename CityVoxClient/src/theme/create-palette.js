// color design tokens export
import { common } from '@mui/material/colors';
import { alpha } from '@mui/material/styles';
import { error, indigo, info, neutral, success, warning } from './colors';

export const colorTokens = {
      grey: {
        0: "#FFFFFF",
        10: "#F6F6F6",
        50: "#F0F0F0",
        100: "#E0E0E0",
        200: "#C2C2C2",
        300: "#A3A3A3",
        400: "#858585",
        500: "#666666",
        600: "#4D4D4D",
        700: "#333333",
        800: "#121212",
        900: "#0A0A0A",
        1000: "#000000",
        1100: "#cccccc",
      },
      primary: {
        50: "#E6FBFF",
        100: "#CCF7FE",
        200: "#66B2FF",
        300: "#66E6FC",
        400: "#33DDFB",
        500: "#00D5FA",
        600: "#00A0BC",
        700: "#006B7D",
        800: "#00353F",
        900: "#001519",
      },
    

    dashboardPrimary:{
      100: "#040509",
          200: "#080b12",
          300: "#0c101b",
          400: "#f6f4f2", // manually changed
          500: "#141b2d",
          600: "#1F2A40",
          700: "#727681",
          800: "#a1a4ab",
          900: "#d0d1d5",

    },
    dashboardPrimaryGreen:{
 
        100: "#dbf5ee",
        200: "#b7ebde",
        300: "#94e2cd",
        400: "#70d8bd",
        500: "#4cceac",
        600: "#3da58a",
        700: "#2e7c67",
        800: "#1e5245",
        900: "#0f2922",

    }
  };
  
// mui theme settings
export const createPalette = (mode) => {
  return mode === "dark"
    ? {
        // palette values for dark mode
        action: {
          active: neutral[500],
          disabled: alpha(neutral[900], 0.38),
          disabledBackground: alpha(neutral[900], 0.12),
          focus: alpha(neutral[900], 0.16),
          hover: alpha(neutral[900], 0.04),
          selected: alpha(neutral[900], 0.12),
        },
        background: {
          default: "#1A2534",
          paper: colorTokens.grey[800],
        },
        divider: '#F2F4F7',
        error,
        info,
        mode: mode,
        neutral,
        primary: indigo,
        success,
        text: {
          primary: "#FFFFFF",
          secondary: neutral[500],
          disabled: alpha("#FFFFFF", 0.38),
        },
        warning,
        button: {
          primary: colorTokens.grey[0],
        }
              ,//primary: {
        //   dark: colorTokens.primary[200],
        //   main: colorTokens.primary[500],
        //   light: colorTokens.primary[800],
        // },
        static: {
          dark: colorTokens.grey[100],
          main: colorTokens.grey[200],
          mediumMain: colorTokens.grey[300],
          medium: colorTokens.grey[400],
          light: colorTokens.grey[700],
          border: "#FFFFFF",
        },
        border:colorTokens.grey[700],
      // background: {
      //   default: colorTokens.grey[1000],
      //   alt: colorTokens.grey[800],
      // },
      dashboard:{
        cardBackground:colorTokens.dashboardPrimary[400],
        cardTextPrimary:colorTokens.dashboardPrimary[100],
        cardTextNumbers:colorTokens.dashboardPrimaryGreen[600],
      }
    }
    : {
        // palette values for light mode
        action: {
          active: neutral[500],
          disabled: alpha(neutral[900], 0.38),
          disabledBackground: alpha(neutral[900], 0.12),
          focus: alpha(neutral[900], 0.16),
          hover: alpha(neutral[900], 0.04),
          selected: alpha(neutral[900], 0.12),
        },
        background: {
          default: common.white,
          paper: common.white,
        },
        divider: '#F2F4F7',
        error,
        info,
        mode: mode,
        neutral,
        primary: indigo,
        success,
        text: {
          primary: neutral[900],
          secondary: neutral[500],
          disabled: alpha(neutral[900], 0.38),
        },
        warning,
        button: {
          primary: colorTokens.grey[1000],
        },
             //primary: {
              //   dark: colorTokens.primary[700],
              //   main: colorTokens.primary[500],
              //   light: colorTokens.primary[50],
              // },
        static: {
          dark: colorTokens.grey[700],
          main: colorTokens.grey[500],
          mediumMain: colorTokens.grey[400],
          medium: colorTokens.grey[300],
          light: colorTokens.grey[50],
          border: colorTokens.grey[1100],
        },
              // background: {
              //   default: colorTokens.grey[0],
              //   alt: colorTokens.grey[10],
              // },
              dashboard:{
                cardBackground:colorTokens.dashboardPrimary[400],
                cardTextPrimary:colorTokens.dashboardPrimary[100],
                cardTextNumbers:colorTokens.dashboardPrimaryGreen[600],
                
              }
      };
    }

  