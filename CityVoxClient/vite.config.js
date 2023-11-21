import { defineConfig } from 'vite'
import viteBasicSslPlugin from '@vitejs/plugin-basic-ssl'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react() , viteBasicSslPlugin()],
  server: {
    https: true,
    hmr: {
      protocol: 'wss',
      host: 'localhost',
      // Optionally specify a port if the default one is not suitable
      //port: 5173,
    },
  },
})
