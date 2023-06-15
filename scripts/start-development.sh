#!/usr/bin/env bash
set -e

PROJECT_DATABASE=src/ScheduleBarbecue.Infrastructure
SQL_CONTEXT_CLASS=ScheduleBarbecue.Infrastructure.Contexts.ScheduleBarbecueContext
PROJECT_API=src/ScheduleBarbecue.Api
LAUNCH_PROFILE=ScheduleBarbecue.Docker

dotnet watch --project ${PROJECT_API} -- run  --launch-profile ${LAUNCH_PROFILE}
