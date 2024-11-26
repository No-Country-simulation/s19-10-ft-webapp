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
import { IoIosLock, IoIosEyeOff, IoIosEye } from "react-icons/io";
import { loginSchemaType } from "./types";
import { loginrSchema } from "./schemas/index";
import { Link } from "react-router-dom";

const LoginForm = () => {
  const [showPassword, setShowPassword] = useState(false);
  const form = useForm<loginSchemaType>({
    resolver: zodResolver(loginrSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  });

  const onSubmit = async (values: loginSchemaType) => {
    console.log(values);
  };
  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="pt-8">
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
        <div className="-mt-3 flex justify-end w-full">
          <Link to="/forgot-password" className="text-sm text-gray-400 text-right">
            Need an account? <span className="text-blue-600 underline">Create one</span>
          </Link>
        </div>
        <div className="w-full pt-3">
          <Button variant={"default"} className="w-full hover:bg-purpleHover">
            Sign in
          </Button>
        </div>
      </form>
    </Form>
  );
};

export default LoginForm;
