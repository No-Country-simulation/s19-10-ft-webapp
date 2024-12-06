import { Link } from 'react-router-dom'
import logo from '@/assets/common/logo-title.svg'

export default function Landingpage() {

  return (
    <div className=''>
      <div className="h-screen bg-[url('@/assets/dashboard/landing-bg.jpg')] flex flex-col justify-between">
        {/* Header */}
        <header className="w-full p-2 flex justify-between items-center">
          <div className='bg-white py-1 rounded-full text-lg font-medium shadow-md transition duration-300'>
        <Link to="/">
          <img src={logo} alt="notepad ai" className="p-4  w-44" />
          </Link>
          </div>
          <nav className="space-x-4 text-black">
            <Link to="/login" className="bg-blue-600 hover:bg-blue-500 text-white px-6 py-3 rounded-full text-lg font-medium shadow-md transition duration-300">
              Sign In
            </Link>
            <Link to="/signup" className="bg-blue-600 hover:bg-blue-500 text-white px-6 py-3 rounded-full text-lg font-medium shadow-md transition duration-300">
              Sign up
            </Link>
          </nav>
        </header>

        {/* Hero Section */}
        <main className="flex flex-col items-center justify-center flex-1 text-center px-5">
          <h2 className="text-5xl font-extrabold mb-5">Interact with Your Documents</h2>
          <p className="text-lg mb-10 max-w-2xl">
            Simplify your workflow with Notepad AI. Use AI to interact with and organize your documents efficiently, powered by .NET and React.
          </p>
          <Link
            to="/dashboard"
            className="bg-blue-600 hover:bg-blue-500 text-white px-6 py-3 rounded-full text-lg font-medium shadow-md transition duration-300"
          >
            Get Started
          </Link>
        </main>

        {/* Footer */}
        <footer className="p-5 text-center text-sm">
          <p>
            © 2024 Notepad AI. All Rights Reserved. Created with ❤️ by
            s19-10-ft-webapp Team.
          </p>
        </footer>
      </div>

    </div>
  )
}