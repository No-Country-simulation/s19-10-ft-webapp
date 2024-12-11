"use client";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  Form,
  Input,
  Button,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui";
import { PiEnvelopeSimpleFill } from "react-icons/pi";
import { FcGoogle } from "react-icons/fc";
import { IoIosLock, IoIosEyeOff, IoIosEye } from "react-icons/io";
import { signupSchemaType } from "./types";
import { signuprSchema } from "./schemas/index";
import { Link, useNavigate } from "react-router-dom";
import { signupUser } from "./actions";
import { toast } from "react-toastify";
import { useMutation } from "@tanstack/react-query";
import Spinner from "@/components/shared/spinner";

const SignUpForm = () => {
  const [showPassword, setShowPassword] = useState(false);
  const navigate = useNavigate();
  
  const form = useForm<signupSchemaType>({
    resolver: zodResolver(signuprSchema),
    defaultValues: {
      name: "",
      lastName: "",
      email: "",
      password: "",
      repeat_password: "",
    },
  });

  const mutation = useMutation({
    mutationFn: signupUser,
    onSuccess: (data) => {
      if (data) {
        navigate("/confirm-password");
      } else {
        toast.error(data.error);
      }
    },
    onError: () => {
      toast.error("Unexpected error occurred");
    },
  });

  const onSubmit = async (values: signupSchemaType) => {
    mutation.mutate(values);
  };
  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="pt-8">
        <FormField
          control={form.control}
          name="name"
          render={({ field }) => (
            <FormItem className="pb-6 relative">
              <FormLabel>Name</FormLabel>
              <PiEnvelopeSimpleFill className=" absolute top-[36px] text-gray ml-2 text-lg" />
              <FormControl>
                <Input
                  placeholder="e.g John"
                  className="mt-1 pl-7 focus-visible:ring-0"
                  {...field}
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="lastName"
          render={({ field }) => (
            <FormItem className="pb-6 relative">
              <FormLabel>Last Name</FormLabel>
              <PiEnvelopeSimpleFill className=" absolute top-[36px] text-gray ml-2 text-lg" />
              <FormControl>
                <Input
                  placeholder="e.g Doe"
                  className="mt-1 pl-7 focus-visible:ring-0"
                  {...field}
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="email"
          render={({ field }) => (
            <FormItem className="pb-6 relative">
              <FormLabel>Email address</FormLabel>
              <PiEnvelopeSimpleFill className=" absolute top-[36px] text-gray ml-2 text-lg" />
              <FormControl>
                <Input
                  placeholder="e.g john@gmail.com"
                  className="mt-1 pl-7 focus-visible:ring-0"
                  {...field}
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="password"
          render={({ field }) => (
            <FormItem className="pb-6 relative">
              <IoIosLock className="absolute top-[42px] text-gray ml-2 text-lg" />
              <IoIosEyeOff
                onClick={() => setShowPassword(!showPassword)}
                className={`${
                  !showPassword ? "block" : "hidden"
                } absolute text-gray right-0 top-[36.6px] mr-2 hover:cursor-pointer`}
              />
              <IoIosEye
                onClick={() => setShowPassword(!showPassword)}
                className={`${
                  showPassword ? "block" : "hidden"
                } absolute text-gray right-0 top-[36.6px] mr-2 hover:cursor-pointer`}
              />
              <FormLabel>Password</FormLabel>
              <FormControl>
                <Input
                  placeholder="At least 8 characters"
                  type={showPassword ? "text" : "password"}
                  className="mt-1 pl-7 focus-visible:ring-0"
                  {...field}
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="repeat_password"
          render={({ field }) => (
            <FormItem className="pb-6 relative">
              <IoIosLock className="absolute top-[42px] text-gray ml-2 text-lg" />
              <IoIosEyeOff
                onClick={() => setShowPassword(!showPassword)}
                className={`${
                  !showPassword ? "block" : "hidden"
                } absolute text-gray right-0 top-[36.6px] mr-2 hover:cursor-pointer`}
              />
              <IoIosEye
                onClick={() => setShowPassword(!showPassword)}
                className={`${
                  showPassword ? "block" : "hidden"
                } absolute text-gray right-0 top-[36.6px] mr-2 hover:cursor-pointer`}
              />
              <FormLabel>Repeat password</FormLabel>
              <FormControl>
                <Input
                  placeholder="Confirm password"
                  type={showPassword ? "text" : "password"}
                  className="mt-1 pl-7 focus-visible:ring-0"
                  {...field}
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <div className="w-full pt-3">
        <Button variant={"default"} className="w-full hover:bg-purpleHover">
            {mutation.isPending ? <Spinner /> : "Register"}
          </Button>
          <div className="flex w-full items-center gap-2 my-5">
            <hr className="h-[2px] bg-gray-200 w-full" />
            <span>Or</span>
            <hr className="h-[2px] bg-gray-200 w-full" />
          </div>
          <Button className="w-full bg-white text-black border-2 border-gray-300 hover:bg-purpleHover">
            Sign in with Google <FcGoogle />
          </Button>
        </div>
        <div className="mt-8 flex justify-end w-full">
          <Link
            to="/login"
            className="text-sm text-gray-400 text-right"
          >
            
Do you already have an account?{" "}
            <span className="text-blue-600 underline">Sign In</span>
          </Link>
        </div>
      </form>
    </Form>
  );
};

export default SignUpForm;
