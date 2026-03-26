using MediatR;

namespace AuthApi.Core.Commands;

public interface ICommand : ICommandBase, IRequest<Unit>
{
}

public interface ICommand<TResponse> : ICommandBase, IRequest<TResponse>
{ 
}

public interface ICommandBase
{
}