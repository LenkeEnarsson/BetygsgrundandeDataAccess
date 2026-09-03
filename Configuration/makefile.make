# Makefile for AppWebApi database migrations (EF Core, SQL Server)
#
# Usage:
#   make migration-add NAME=AddCustomerTable   Create a new migration
#   make migrate                               Apply pending migrations to the database
#   make migration-remove                      Remove the last (not-yet-applied) migration
#   make migration-list                        List all migrations
#   make seed                                  Run the database seed (placeholder)
#   make help                                  Show this help

PROJECT ?= .
STARTUP_PROJECT ?= .
CONFIGURATION ?= Debug

.DEFAULT_GOAL := help

.PHONY: help migration-add migrate migration-remove migration-list seed

help: ## Show this help
	@echo "Available targets:"
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "  \033[36m%-18s\033[0m %s\n", $$1, $$2}'

migration-add: ## Create a new migration. Usage: make migration-add NAME=MigrationName
ifndef NAME
	$(error NAME is required, e.g. make migration-add NAME=AddCustomerTable)
endif
	dotnet ef migrations add $(NAME) \
		--project $(PROJECT) \
		--startup-project $(STARTUP_PROJECT) \
		--configuration $(CONFIGURATION)

migrate: ## Apply all pending migrations to the database
	dotnet ef database update \
		--project $(PROJECT) \
		--startup-project $(STARTUP_PROJECT) \
		--configuration $(CONFIGURATION)

migration-remove: ## Remove the last migration (must not already be applied to the database)
	dotnet ef migrations remove \
		--project $(PROJECT) \
		--startup-project $(STARTUP_PROJECT)

migration-list: ## List all migrations
	dotnet ef migrations list \
		--project $(PROJECT) \
		--startup-project $(STARTUP_PROJECT)

seed: ## Seed the database with initial/test data (placeholder — adjust to your actual seed entry point)
	dotnet run --project $(PROJECT) --configuration $(CONFIGURATION) -- seed
