#!/bin/sh

PROJECT=src/ScheduleBarbecue.Infrastructure
BUILD_PROJECT=src/ScheduleBarbecue.Api
SQL_CONTEXT_CLASS=ScheduleBarbecueContext

dotnet ef database update --project ${PROJECT} --startup-project ${BUILD_PROJECT} --context ${SQL_CONTEXT_CLASS}