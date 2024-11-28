import { createRoot } from 'react-dom/client'
import Router from './routes/index.tsx'
import './index.css'
import 'react-toastify/dist/ReactToastify.css';
import ReactQueryProvider from './util/react-query.provider.tsx';
import { ToastContainer } from 'react-toastify';

createRoot(document.getElementById("root")!).render(
  <>
    <ReactQueryProvider>
      <Router />
      <ToastContainer />
    </ReactQueryProvider>
  </>
);
