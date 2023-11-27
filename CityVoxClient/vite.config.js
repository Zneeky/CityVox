import { defineConfig } from 'vite'
import viteBasicSslPlugin from '@vitejs/plugin-basic-ssl'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react() , viteBasicSslPlugin()],
  server: {
    https: true,
    strictPort: true, // Ensure Vite uses the specified port
    port: 5173, // Specify your port
    hmr: {
      clientPort: 5173, // Same as server port
    },
  },
})
