using System.Collections.Generic;
using UnityEngine;

public class Command : MonoBehaviour
{
    public Actor currentActor;

	private ICommand moveRight;
	private ICommand moveLeft;
	private ICommand search;
	private ICommand takeStair;

	private void Awake()
	{
		moveRight = new MoveRight();
		moveLeft = new MoveLeft();
		search = new Search();
		takeStair = new TakeStair();
	}

	public void MoveRight()
	{
		moveRight.Execute(currentActor);
	}
	public void MoveLeft()
	{
		moveLeft.Execute(currentActor);
	}
	public void Search()
	{
		search.Execute(currentActor);
	}
	public void TakeStair()
	{
		takeStair.Execute(currentActor);
	}
}


public interface ICommand // ALl Commands available
{
	void Execute(Actor actor);
}
public class MoveRight : ICommand
{
	public void Execute(Actor actor)
	{
		actor.MoveRight();
	}
}
public class MoveLeft : ICommand
{
	public void Execute(Actor actor)
	{
		actor.MoveLeft();
	}
}
	
public class Search : ICommand
{
	public void Execute(Actor actor)
	{
		actor.Search();
	}
}
	
public class TakeStair : ICommand
{
	public void Execute(Actor actor)
	{
		actor.TakeStair();
	}
}



public abstract class Actor : MonoBehaviour
{
	public abstract void MoveRight();
	public abstract void MoveLeft();
	public abstract void Search();
	public abstract void TakeStair();
}