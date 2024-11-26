import { createRoot } from 'react-dom/client'
import Router from './routes/index.tsx'
import './index.css'

createRoot(document.getElementById('root')!).render(
  <>
    <Router />
  </>,
)
